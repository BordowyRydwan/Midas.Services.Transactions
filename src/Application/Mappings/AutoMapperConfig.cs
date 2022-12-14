using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public static class AutoMapperConfig
{
    private static MapperConfigurationExpression Config => GetConfig();

    private static MapperConfigurationExpression GetConfig()
    {
        var result = new MapperConfigurationExpression();

        result.CreateMap<AddInvoiceDto, Invoice>()
            .ForPath(dest => dest.Transaction, act => act.Ignore());

        result.CreateMap<Currency, CurrencyDto>().ReverseMap();
        result.CreateMap<ICollection<Currency>, CurrencyListDto>()
            .ForMember(dest => dest.Items, act => act.MapFrom(src => src))
            .ForMember(dest => dest.Count, act => act.MapFrom(src => src.Count));
        
        result.CreateMap<TransactionCategory, TransactionCategoryDto>().ReverseMap();
        result.CreateMap<ICollection<TransactionCategory>, TransactionCategoryListDto>()
            .ForMember(dest => dest.Items, act => act.MapFrom(src => src))
            .ForMember(dest => dest.Count, act => act.MapFrom(src => src.Count));

        result.CreateMap<Invoice, InvoiceDto>()
            .ForMember(dest => dest.FileMetadata, act => act.Ignore());
        result.CreateMap<ICollection<Invoice>, InvoiceListDto>()
            .ForMember(dest => dest.Items, act => act.MapFrom(src => src))
            .ForMember(dest => dest.Count, act => act.MapFrom(src => src.Count));

        result.CreateMap<Transaction, TransactionDto>();
        result.CreateMap<TransactionDto, Transaction>()
            .ForMember(dest => dest.Currency, act => act.Ignore())
            .ForMember(dest => dest.TransactionCategory, act => act.Ignore())
            .ForMember(dest => dest.CurrencyCode, act => act.MapFrom(src => src.Currency.Code))
            .ForMember(dest => dest.TransactionCategoryId, act => act.MapFrom(src => src.TransactionCategory.Id));
        result.CreateMap<ICollection<TransactionDto>, TransactionListDto>()
            .ForMember(dest => dest.Items, act => act.MapFrom(src => src))
            .ForMember(dest => dest.Count, act => act.MapFrom(src => src.Count));
        result.CreateMap<ICollection<Transaction>, TransactionListDto>()
            .ForMember(dest => dest.Items, act => act.MapFrom(src => src))
            .ForMember(dest => dest.Count, act => act.MapFrom(src => src.Count));

        return result;
    }

    public static IMapper Initialize()
    {
        return new MapperConfiguration(Config).CreateMapper();
    }
}