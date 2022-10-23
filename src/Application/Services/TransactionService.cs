using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Midas.Services.Family;
using Midas.Services.User;

namespace Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly IUserClient _userClient;
    private readonly IFamilyClient _familyClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IMapper mapper, 
        IUserClient userClient, 
        IFamilyClient familyClient,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _userClient = userClient;
        _familyClient = familyClient;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task AddTransaction(TransactionDto transaction)
    {
        var activeUser = await _userClient.GetActiveUserAsync().ConfigureAwait(false);
        var userFamilyRoles = await _familyClient.GetFamilyMembershipsForUserAsync().ConfigureAwait(false);
        var canChangeValue = new List<bool>
        {
            (ulong)activeUser.Id == transaction.Id,
            userFamilyRoles.Items.FirstOrDefault(x => x.User.Id == activeUser.Id).FamilyRole.Name ==
            "Main administrator"
        };
        
        if (!canChangeValue.All(x => x))
        {
            throw new Exception();
        }
        
        var entity = _mapper.Map<TransactionDto, Transaction>(transaction);
        await _transactionRepository.AddTransaction(entity).ConfigureAwait(false);
    }

    public async Task DeleteTransaction(ulong transactionId)
    {
        var activeUser = await _userClient.GetActiveUserAsync().ConfigureAwait(false);
        var userFamilyRoles = await _familyClient.GetFamilyMembershipsForUserAsync().ConfigureAwait(false);
        var canChangeValue = new List<bool>
        {
            (ulong)activeUser.Id == transactionId,
            userFamilyRoles.Items.FirstOrDefault(x => x.User.Id == activeUser.Id).FamilyRole.Name ==
            "Main administrator"
        };
        
        if (!canChangeValue.All(x => x))
        {
            throw new Exception();
        }
        
        await _transactionRepository.DeleteTransaction(transactionId).ConfigureAwait(false);
    }

    public async Task ModifyTransaction(TransactionDto transaction)
    {
        var entity = _mapper.Map<TransactionDto, Transaction>(transaction);
        await _transactionRepository.ModifyTransaction(entity).ConfigureAwait(false);
    }

    public async Task<TransactionListDto> GetTransactionsForUser(ulong userId)
    {
        var transactions = await _transactionRepository.GetTransactionsForUser(userId).ConfigureAwait(false);
        var result = _mapper.Map<ICollection<Transaction>, TransactionListDto>(transactions);

        return result;
    }

    public async Task<TransactionListDto> GetTransactionsForUserBetweenDates(GetTransactionsForUserBetweenDatesDto dto)
    {
        var transactions = await _transactionRepository
            .GetTransactionsForUserBetweenDates(dto.UserId, dto.DateFrom, dto.DateTo)
            .ConfigureAwait(false);
        
        var result = _mapper.Map<ICollection<Transaction>, TransactionListDto>(transactions);
        return result;
    }
}