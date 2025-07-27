using AutoMapper;
using DevSandbox.Shared.Kernel.Extensions;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Application.Features.Users.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // User → UserListDto
        CreateMap<ApplicationUser, UserListDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (int)src.Role))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.GetDisplayName()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.GetDisplayName()));

        // User → UserDetailsDto
        CreateMap<ApplicationUser, UserDetailsDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (int)src.Role))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.GetDisplayName()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.GetDisplayName()))
            .ForMember(dest => dest.LoginLogs, opt => opt.MapFrom(src => src.LoginLogs));

        // User → UserDto
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (int)src.Role))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status));

        // User → UpdateUserDto
        CreateMap<ApplicationUser, UpdateUserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (int?)src.Role))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int?)src.Status));

        // CreateUserDto → ApplicationUser
        CreateMap<CreateUserDto, ApplicationUser>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (UserRole)src.Role))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatus.Active))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

        // UpdateUserDto → ApplicationUser
        CreateMap<UpdateUserDto, ApplicationUser>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.HasValue ? (UserRole)src.Role.Value : UserRole.Manager))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.HasValue ? (UserStatus)src.Status.Value : UserStatus.Active))
            .ForMember(dest => dest.UserName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.UserName)));
    }
}
