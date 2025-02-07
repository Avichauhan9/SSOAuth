using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SSO_Backend.Context;
using SSO_Backend.Models;
using SSO_Backend.Utilities;

namespace SSO_Backend.Services;

public class AuthService(UserInfo user)
{
    protected readonly UserInfo _user = user;
}

public class UserInfo
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Name => $"{FirstName} {LastName}";

    public string? AzureADUserId { get; set; }

    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public HashSet<string> Permissions { get; set; } = [];
    public HashSet<string> Roles { get; set; } = [];
    public bool? IsServicePrincipalUser { get; set; }
}

public class CurrentUser : UserInfo
{
    private readonly AppDBContext _context;


    public CurrentUser(AppDBContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        var user = httpContextAccessor.HttpContext!.User ?? throw new UserNotFoundException();
        if (!user.Identity!.IsAuthenticated) throw new UserNotFoundException();

        string uniqueIdentifier = GetUniqueIdentityParameter(user);

        User userDetails = GetUserDetails(uniqueIdentifier);

        if (userDetails != null)
        {
            Id = userDetails.Id;
            FirstName = userDetails.FirstName;
            LastName = userDetails.LastName;
            IsActive = userDetails.IsActive;
            Email = userDetails.Email;
            Permissions = [.. userDetails.UserRoles.SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Code)).Distinct()];
            Roles = [.. userDetails.UserRoles.Select(ur => ur.Role.Name).Distinct()];
            int claimValue = GetClaimValueFromToken(httpContextAccessor.HttpContext.User.Claims);
            IsServicePrincipalUser = claimValue == 1;

            if (!userDetails.IsActive)
                throw new UserInActiveException();
        }

    }
    public User GetUserDetails(string email)
    {
        return _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ThenInclude(r => r.RolePermissions).ThenInclude(rp => rp.Permission).Where(u => u.Email.ToLower() == email.ToLower()).FirstOrDefault() ?? throw new UserNotFoundException();
    }

    public static int GetClaimValueFromToken(IEnumerable<Claim> claims)
    {
        if (claims is null)
            throw new UserNotFoundException();

        var claim = claims.FirstOrDefault(c => c.Type.Equals("appidacr", StringComparison.OrdinalIgnoreCase)
        || c.Type.Equals("azpacr", StringComparison.OrdinalIgnoreCase));

        if (claim != null && int.TryParse(claim.Value, out int value))
        {
            return value;
        }
        throw new UserNotFoundException();
    }

    public static string GetUniqueIdentityParameter(ClaimsPrincipal user)
    {
        var claims = user.Claims;
        int claimValue = GetClaimValueFromToken(claims);

        if (claimValue == 1)
        {
            var clientId = user.FindFirst("azp")?.Value;
            return clientId ?? "";
        }
        else
        {
            var uniqueIdentifier = user.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(uniqueIdentifier))
            {
                uniqueIdentifier = user.FindFirst("preferred_username")?.Value;
            }

            return uniqueIdentifier ?? "";
        }

    }

}
