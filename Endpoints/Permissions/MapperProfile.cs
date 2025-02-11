
using AutoMapper;
using SSO_Backend.Endpoints.Permissions.DTO;
using SSO_Backend.Models;

namespace SSO_Backend.Endpoints.Permissions;


public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreatePermissionDTO, Permission>()
            .ForMember(
                dest => dest.Code,
                opt => opt.MapFrom(src => src.PermissionCode)
            ).ForMember(dest=>dest.Name,opt=>opt.MapFrom(src=>src.PermissionName));

        CreateMap<Role, RolesByPermissionDTO>()
            .ForMember(
                dest => dest.RoleId,
                opt => opt.MapFrom(src => src.Id)
            ).ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));

        CreateMap<Permission,GetAllPermissionDTO>().ForMember(
                dest => dest.PermissionId,
                opt => opt.MapFrom(src => src.Id)
            ).ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest=>dest.PermissionCode, opt=>opt.MapFrom(src=>src.Code));

        CreateMap<Permission, PermissionByIdDTO>().ForMember(
                dest => dest.PermissionCode,
                opt => opt.MapFrom(src => src.Code)
            ).ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Name));
    }
}
