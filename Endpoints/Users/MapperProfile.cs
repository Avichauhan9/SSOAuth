
using AutoMapper;
using SSO_Backend.Endpoints.Users.DTO;
using SSO_Backend.Models;

namespace SSO_Backend.Endpoints.Users;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDTO>()
            .ForMember(
                dest => dest.Permissions,
                opt => opt.MapFrom(src => src.UserRoles.SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Code)).Distinct())
            )
            .ForMember(
                dest => dest.Roles,
                opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name).Distinct())
            );

        CreateMap<User, AllUserDTO>();
        CreateMap<CreateUserDTO, User>();
    }
}
