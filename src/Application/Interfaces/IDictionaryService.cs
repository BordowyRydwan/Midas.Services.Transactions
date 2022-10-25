using Application.Dto;

namespace Application.Interfaces;

public interface IDictionaryService
{
    public Task<CurrencyListDto> GetCurrencies();
    public Task<TransactionCategoryListDto> GetTransactionCategories();
}