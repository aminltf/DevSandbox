﻿using AutoMapper;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Application.Features.Users.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, ApplicationUser>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (UserRole)src.Role))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatus.Active));

        CreateMap<ApplicationUser, GetUserByIdResultDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<ApplicationUser, UserListDto>()
             .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
             .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()))
             .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => src.LastModifiedBy != null ? src.LastModifiedAt : null));
    }
}
