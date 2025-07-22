using AutoMapper;
using CleanArchitectureDemo.Application.Features.Products.Dtos;
using CleanArchitectureDemo.Domain.Entities;
using CleanArchitectureDemo.Domain.Enums;
using CleanArchitectureDemo.Domain.ValueObjects;
using DevSandbox.Shared.Kernel.Extensions;
using DevSandbox.Shared.Kernel.Paging;

namespace CleanArchitectureDemo.Application.Features.Products.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // Create
        CreateMap<Product, CreateProductDto>()
            // Entity -> DTO
            .ForMember(dest => dest.PriceAmount,
                        opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.PriceCurrency,
                        opt => opt.MapFrom(src => src.Price.Currency))
            .ForMember(dest => dest.Status,
                        opt => opt.MapFrom(src => (int)src.Status))
            .ReverseMap()
            // DTO -> Entity
            .ForMember(dest => dest.Price,
                        opt => opt.MapFrom(src => new Money(src.PriceAmount, src.PriceCurrency)))
            .ForMember(dest => dest.Status,
                        opt => opt.MapFrom(src => (ProductStatus)src.Status));

        // Update
        CreateMap<Product, UpdateProductDto>()
            // Entity -> DTO
            .ForMember(dest => dest.PriceAmount,
                        opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.PriceCurrency,
                        opt => opt.MapFrom(src => src.Price.Currency))
            .ForMember(dest => dest.Status,
                        opt => opt.MapFrom(src => (int)src.Status))
            .ReverseMap()
            // DTO -> Entity
            .ForMember(dest => dest.Price,
                        opt => opt.MapFrom(src => new Money(src.PriceAmount, src.PriceCurrency)))
            .ForMember(dest => dest.Status,
                        opt => opt.MapFrom(src => (ProductStatus)src.Status));

        // GetById
        CreateMap<Product, ProductDetailDto>()
            .ForMember(dest => dest.PriceAmount,
                        opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.PriceCurrency,
                        opt => opt.MapFrom(src => src.Price.Currency))
            .ForMember(dest => dest.Status,
                        opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.StatusTitle,
                        opt => opt.MapFrom(src => src.Status.GetDisplayName()));

        // Get
        CreateMap<Product, ProductListDto>()
            .ForMember(dest => dest.PriceAmount,
                        opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.PriceCurrency,
                        opt => opt.MapFrom(src => src.Price.Currency))
            .ForMember(dest => dest.Status,
                        opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.StatusTitle,
                        opt => opt.MapFrom(src => src.Status.GetDisplayName()));

        // Report
        CreateMap<Product, ProductReportDto>()
            .ForMember(dest => dest.PriceAmount,
                        opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.PriceCurrency,
                        opt => opt.MapFrom(src => src.Price.Currency))
            .ForMember(dest => dest.StatusTitle,
                        opt => opt.MapFrom(src => src.Status.GetDisplayName()));

    }
}
