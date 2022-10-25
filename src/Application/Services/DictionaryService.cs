using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class DictionaryService : IDictionaryService
{
    private readonly IDictionaryRepository _dictionaryRepository;
    private readonly IMapper _mapper;

    public DictionaryService(IDictionaryRepository dictionaryRepository, IMapper mapper)
    {
        _dictionaryRepository = dictionaryRepository;
        _mapper = mapper;
    }
    
    public async Task<CurrencyListDto> GetCurrencies()
    {
        var currenciesList = await _dictionaryRepository.GetCurrencies().ConfigureAwait(false);
        var mappedDto = _mapper.Map<ICollection<Currency>, CurrencyListDto>(currenciesList);

        return mappedDto;
    }

    public async Task<TransactionCategoryListDto> GetTransactionCategories()
    {
        var categoriesList = await _dictionaryRepository.GetTransactionCategories().ConfigureAwait(false);
        var mappedDto = _mapper.Map<ICollection<TransactionCategory>, TransactionCategoryListDto>(categoriesList);

        return mappedDto;
    }
}