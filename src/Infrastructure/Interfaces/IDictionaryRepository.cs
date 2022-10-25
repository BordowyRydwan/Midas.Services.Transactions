using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IDictionaryRepository
{
    public Task<ICollection<Currency>> GetCurrencies();
    public Task<ICollection<TransactionCategory>> GetTransactionCategories();
}