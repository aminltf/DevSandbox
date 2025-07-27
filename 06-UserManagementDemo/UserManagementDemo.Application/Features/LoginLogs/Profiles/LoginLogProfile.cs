using AutoMapper;
using UserManagementDemo.Application.Features.LoginLogs.Dtos;
using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Application.Features.LoginLogs.Profiles;

public class LoginLogProfile : Profile
{
    public LoginLogProfile()
    {
        // LoginLog → LoginLogDto
        CreateMap<LoginLog, LoginLogDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
    }
}
