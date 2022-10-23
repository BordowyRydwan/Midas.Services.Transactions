using Application.Dto;

namespace Application.Interfaces;

public interface ITransactionService
{
    public Task AddTransaction(TransactionDto transaction);
    public Task DeleteTransaction(ulong transactionId);
    public Task ModifyTransaction(TransactionDto transaction);

    public Task<TransactionListDto> GetTransactionsForUser(ulong userId);
    public Task<TransactionListDto> GetTransactionsForUserBetweenDates(GetTransactionsForUserBetweenDatesDto dto);
}