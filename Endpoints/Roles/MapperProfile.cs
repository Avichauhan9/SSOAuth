
using AutoMapper;
using SSO_Backend.Endpoints.Roles.DTO;
using SSO_Backend.Models;

namespace SSO_Backend.Endpoints.Roles;


public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Role, RoleDTO>()
          .ForMember(
              dest => dest.Permissions,
              opt => opt.MapFrom(src => src.RolePermissions != null
            ? src.RolePermissions
                .Select(ur => new PermissionDTO
                {
                    Name = ur.Permission.Name,
                    Id = ur.Permission.Id
                })
                .ToList()
            : new List<PermissionDTO>() // Handle null case
             )
          );
        CreateMap<Role, AllRolesDTO>();
        CreateMap<CreateRoleDTO, Role>();
        CreateMap<User, UserByRoleDTO>();
    }
}
