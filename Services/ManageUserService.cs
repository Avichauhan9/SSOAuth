using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Models;
using SSO_Backend.Context;
using SSO_Backend.Endpoints.Users.DTO;
using SSO_Backend.Models;
using SSO_Backend.Models.AppSettings;
using SSO_Backend.Models.Mailing;
using SSO_Backend.Result;
using User = SSO_Backend.Models.User;

namespace SSO_Backend.Services;

public class ManageUserService(AppDBContext iamDbConext, IGraphService graphService, IMapper mapper, IEmailService emailServices, IOptions<AzureADConfig> azureADConfig, UserInfo user) : BaseService(iamDbConext, user), IManageUserService
{
    private readonly IGraphService _graphService = graphService;
    private readonly IMapper _mapper = mapper;
    private readonly IEmailService _emailServices = emailServices;
    private readonly AzureADConfig _azureADConfig = azureADConfig.Value;

    public async Task<Result<UserDTO>> GetUserByEmail(string email)
    {
        var existingUser = await _context.Users.Include(u => u.UserRoles).ThenInclude(userRole => userRole.Role)          // Include Role in UserRoles
            .ThenInclude(role => role.RolePermissions)
                .ThenInclude(rolePermission => rolePermission.Permission).SingleOrDefaultAsync(s => s.Email.ToLower() == email.ToLower());

        if (existingUser == null)
            return Result<UserDTO>.Invalid([new ValidationError { Key = "User", ErrorMessage = $"User is not found." }]);

        return Result<UserDTO>.Success(_mapper.Map<UserDTO>(existingUser));
    }

    public async Task<Result<UserDTO>> GetUserById(int Id)
    {
        var existingUser = await _context.Users.Include(u => u.UserRoles).ThenInclude(userRole => userRole.Role)          // Include Role in UserRoles
            .ThenInclude(role => role.RolePermissions)
                .ThenInclude(rolePermission => rolePermission.Permission).SingleOrDefaultAsync(s => s.Id == Id);

        if (existingUser == null)
            return Result<UserDTO>.Invalid([new ValidationError { Key = "User", ErrorMessage = $"User is not found." }]);

        return Result<UserDTO>.Success(_mapper.Map<UserDTO>(existingUser));
    }

    public async Task<Result<IEnumerable<AllUserDTO>>> GetUsers(bool? isActive, string currentTenantId)
    {
        if (isActive == null)
        {
            var users = await _context.Users.ToListAsync();
            return Result<IEnumerable<AllUserDTO>>.Success(_mapper.Map<IEnumerable<AllUserDTO>>(users));
        }
        else
        {
            var users = await _context.Users.Where(u => u.IsActive == isActive).ToListAsync();
            return Result<IEnumerable<AllUserDTO>>.Success(_mapper.Map<IEnumerable<AllUserDTO>>(users));
        }
    }

    public async Task<Result<int>> ModifyUserStatus(UpdateUserStatusDTO changeStatusRequest)
    {

        var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == changeStatusRequest.Id);
        if (existingUser == null)
            return Result<int>.Invalid([new ValidationError { Key = "User", ErrorMessage = $"User is not found." }]);
        else if(string.IsNullOrWhiteSpace(existingUser.AzureADUserId))
            return Result<int>.Invalid([new ValidationError { Key = "User", ErrorMessage = $"User registration is malicious" }]);

        await _graphService.DisabledUser(existingUser.AzureADUserId, changeStatusRequest.AccountEnabled);
        existingUser.IsActive = changeStatusRequest.AccountEnabled;
        existingUser.UpdatedById = _user.Id;
        await _context.SaveChangesAsync();
        return Result<int>.Success(200);
    }

    public async Task<Result<UserDTO>> CreateUser(CreateUserDTO userRequest)
    {
        var isEmailExists = await DoesEmailExist(userRequest.Email);
        if (!isEmailExists.IsSuccess)
        {
            return Result<UserDTO>.Invalid(isEmailExists.ValidationErrors);
        }

        var transaction = await _context.Database.BeginTransactionAsync();

        var user = _mapper.Map<User>(userRequest);

        var azureAdUser = await _graphService.GetUserByEmail(user.Email);
        if (azureAdUser == null)
        {

            var invitation = await _graphService.InviteGuestUser(user);

            if (invitation == null || invitation.InvitedUser == null || invitation.InvitedUser.Id == null)
                return Result<UserDTO>.Invalid([new() { Key = "User", ErrorMessage = $"{userRequest.Email} is not added in Azure AD." }]);

            user.AzureADUserId = invitation.InvitedUser.Id;
            user.CreatedById = _user.Id;
            user.UpdatedById = _user.Id;
            user.IsActive = true;
        }
        else
        {
            user.AzureADUserId = azureAdUser.Id;
        }

        if (!string.IsNullOrWhiteSpace(user.AzureADUserId))
            await _graphService.UserAssignment(user.AzureADUserId);

        await _graphService.UpdateUser(user);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        if (userRequest.RoleIds?.Count > 0)
        {
            var userRoles = userRequest.RoleIds.Select(roleId => new UserRole { RoleId = roleId, UserId = user.Id, CreatedById = _user.Id, UpdatedById = _user.Id }).ToList();
            await _context.UserRoles.AddRangeAsync(userRoles);
        }

        await _context.SaveChangesAsync();

        var userInvite = EmailService.GetFluidTemplate("InviteUser", _context);
        if (userInvite == null)
        {
            return Result<UserDTO>.Invalid([new() { Key = "User", ErrorMessage = $"Email template is not found." }]);
        }
        var userObj = new
        {
            DisplayName = $"{userRequest.FirstName} {userRequest.LastName}",
            InviteLink = _azureADConfig.InviteRedirectUrl,
            AppName = _azureADConfig.ClientAppName
        };
        userInvite.To.Add(userRequest.Email);
        await _emailServices.SendEmailWithTemplateAsync(userInvite, userObj);
        await transaction.CommitAsync();
        return Result<UserDTO>.Success(_mapper.Map<UserDTO>(user));
    }

    private async Task<Result<bool>> DoesEmailExist(string email)
    {
        var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        return existingUser != null;
    }

    public async Task<Result<IEnumerable<AllUserDTO>>> GetUsers(bool? isActive)
    {
        if (isActive == null)
        {
            var users = await _context.Users.ToListAsync();
            return Result<IEnumerable<AllUserDTO>>.Success(_mapper.Map<IEnumerable<AllUserDTO>>(users));
        }
        else
        {
            var users = await _context.Users.Where(u => u.IsActive == isActive).ToListAsync();
            return Result<IEnumerable<AllUserDTO>>.Success(_mapper.Map<IEnumerable<AllUserDTO>>(users));
        }
    }

    public async Task<Result<int>> UpdateUser(UpdateUserDTO userRequest)
    {
        var existingUser = await _context.Users
        .Include(s => s.UserRoles)
        .SingleOrDefaultAsync(u => u.Id == userRequest.Id);

        if (existingUser == null)
            return Result<int>.Invalid([new ValidationError { Key = "User", ErrorMessage = $"User is not found." }]);

        if (!existingUser.IsActive)
            return Result<int>.Invalid([new ValidationError { Key = "User", ErrorMessage = $"You can not change inactive user data." }]);

        if (!string.IsNullOrWhiteSpace(userRequest.FirstName))
        {
            existingUser.FirstName = userRequest.FirstName;
        }
        if (!string.IsNullOrWhiteSpace(userRequest.LastName))
        {
            existingUser.LastName = userRequest.LastName;
        }
        if (userRequest.IsServicePrincipal != null)
        {
            existingUser.IsServicePrincipal = (bool)userRequest.IsServicePrincipal;
        }

        if (userRequest.RoleIds?.Count > 0)
        {
            _context.UserRoles.RemoveRange(existingUser.UserRoles);
            var userRoles = userRequest.RoleIds.Select(roleId => new UserRole { RoleId = roleId, UserId = existingUser.Id, CreatedById = _user.Id, UpdatedById = _user.Id }).ToList();
            await _context.UserRoles.AddRangeAsync(userRoles);
        }
        existingUser.UpdatedById = _user.Id;

        await _context.SaveChangesAsync();
        return Result<int>.Success(200);
    }
}
