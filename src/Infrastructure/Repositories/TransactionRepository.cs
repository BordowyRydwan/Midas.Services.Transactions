using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly TransactionDbContext _context;

    public TransactionRepository(TransactionDbContext context)
    {
        _context = context;
    }

    public async Task AddTransaction(Transaction transaction)
    {
        if (transaction is null)
        {
            throw new ArgumentNullException("Transaction argument is null");
        }
        
        transaction.DateCreated = DateTime.UtcNow;
        transaction.DateModified = DateTime.UtcNow;
        
        await _context.Transactions.AddAsync(transaction).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteTransaction(ulong transactionId)
    {
        var existingEntity = await _context.Transactions
            .Include(x => x.Currency)
            .Include(x => x.TransactionCategory)
            .SingleOrDefaultAsync(x => x.Id == transactionId)
            .ConfigureAwait(false);

        if (existingEntity is null)
        {
            throw new NullReferenceException($"Transaction with ID: {transactionId} does not exist");
        }

        _context.Transactions.Remove(existingEntity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task ModifyTransaction(Transaction transaction)
    {
        var existingEntity = await _context.Transactions
            .Include(x => x.Currency)
            .Include(x => x.TransactionCategory)
            .SingleOrDefaultAsync(x => x.Id == transaction.Id)
            .ConfigureAwait(false);

        if (existingEntity is null)
        {
            throw new NullReferenceException($"Transaction with ID: {transaction.Id} does not exist");
        }

        existingEntity.Title = transaction.Title;
        existingEntity.Description = transaction.Description;
        existingEntity.RecipientName = transaction.RecipientName;
        existingEntity.Amount = transaction.Amount;
        existingEntity.CurrencyCode = transaction.CurrencyCode;
        existingEntity.TransactionCategoryId = transaction.TransactionCategoryId;
        existingEntity.DateModified = DateTime.UtcNow;

        _context.Transactions.Update(existingEntity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<ICollection<Transaction>> GetTransactionsForUser(ulong userId)
    {
        var transactionList = await _context.Transactions
            .Include(x => x.Currency)
            .Include(x => x.TransactionCategory)
            .Include(x => x.Invoices)
            .Where(x => x.UserId == userId)
            .ToListAsync()
            .ConfigureAwait(false);

        return transactionList ?? new List<Transaction>();
    }
    
    public async Task<Transaction> GetTransaction(ulong transactionId)
    {
        var transactionList = await _context.Transactions
            .Include(x => x.Currency)
            .Include(x => x.TransactionCategory)
            .Include(x => x.Invoices)
            .SingleOrDefaultAsync(x => x.Id == transactionId)
            .ConfigureAwait(false);

        return transactionList;
    }

    public async Task<ICollection<Transaction>> GetTransactionsForUserBetweenDates(ulong userId, DateTime dateFrom, DateTime dateTo)
    {
        if (dateFrom > dateTo)
        {
            throw new ArgumentException($"End date ({dateFrom}) is later than start date ({dateTo})");
        }
        
        var transactionList = await _context.Transactions
            .Include(x => x.Currency)
            .Include(x => x.TransactionCategory)
            .Include(x => x.Invoices)
            .Where(x => x.UserId == userId && x.DateCreated >= dateFrom && x.DateCreated <= dateTo)
            .ToListAsync()
            .ConfigureAwait(false);

        return transactionList;
    }
}