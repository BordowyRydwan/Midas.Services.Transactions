using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ITransactionRepository : IRepository
{
    public Task AddTransaction(Transaction transaction);
    public Task DeleteTransaction(ulong transactionId);
    public Task ModifyTransaction(Transaction transaction);

    public Task<ICollection<Transaction>> GetTransactionsForUser(ulong userId);
    public Task<ICollection<Transaction>> GetTransactionsForUserBetweenDates(ulong userId, DateTime dateFrom, DateTime dateTo);
}