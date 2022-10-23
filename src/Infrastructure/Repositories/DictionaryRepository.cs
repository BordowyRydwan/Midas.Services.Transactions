using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DictionaryRepository : IDictionaryRepository
{
    private readonly TransactionDbContext _context;
    
    public DictionaryRepository(TransactionDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Currency>> GetCurrencies()
    {
        return await _context.Currencies.ToListAsync().ConfigureAwait(false);
    }

    public async Task<ICollection<TransactionCategory>> GetTransactionCategories()
    {
        return await _context.TransactionCategories.ToListAsync().ConfigureAwait(false);
    }
}