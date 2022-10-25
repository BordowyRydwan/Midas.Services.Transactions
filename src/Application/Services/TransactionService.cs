using Application.Dto;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Midas.Services.Family;
using Midas.Services.FileStorage;
using Midas.Services.User;

namespace Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly IUserClient _userClient;
    private readonly IFamilyClient _familyClient;
    private readonly IFileStorageClient _fileClient;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IMapper mapper, 
        IUserClient userClient, 
        IFamilyClient familyClient,
        IFileStorageClient fileClient
    )
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _userClient = userClient;
        _familyClient = familyClient;
        _fileClient = fileClient;
    }
    
    public async Task AddTransaction(TransactionDto transaction)
    {
        var permissionCheckArgs = GenerateDefaultPermissionCheckArgs(transaction.Id);
        var canChangeValue = await PermissionHelper.IsTransactionOwnedByUserOrHisFamily(permissionCheckArgs);
        
        if (!canChangeValue)
        {
            throw new Exception();
        }
        
        var entity = _mapper.Map<TransactionDto, Transaction>(transaction);
        await _transactionRepository.AddTransaction(entity).ConfigureAwait(false);
    }

    public async Task DeleteTransaction(ulong transactionId)
    {
        var permissionCheckArgs = GenerateDefaultPermissionCheckArgs(transactionId);
        var canChangeValue = await PermissionHelper.IsTransactionOwnedByUserOrHisFamily(permissionCheckArgs);
        
        if (!canChangeValue)
        {
            throw new Exception();
        }
        
        await _transactionRepository.DeleteTransaction(transactionId).ConfigureAwait(false);
    }

    public async Task ModifyTransaction(TransactionDto transaction)
    {
        var permissionCheckArgs = GenerateDefaultPermissionCheckArgs(transaction.Id);
        var canChangeValue = await PermissionHelper.IsTransactionOwnedByUserOrHisFamily(permissionCheckArgs);
        
        if (!canChangeValue)
        {
            throw new Exception();
        }
        
        var entity = _mapper.Map<TransactionDto, Transaction>(transaction);
        await _transactionRepository.ModifyTransaction(entity).ConfigureAwait(false);
    }

    public async Task<TransactionListDto> GetTransactionsForUser(ulong userId)
    {
        var transactions = await _transactionRepository.GetTransactionsForUser(userId).ConfigureAwait(false);
        var result = _mapper.Map<ICollection<Transaction>, TransactionListDto>(transactions);

        foreach (var transaction in result.Items)
        {
            foreach (var invoice in transaction.Invoices.Items)
            {
                var fileMetadata = await _fileClient.GetFileMetadataAsync(invoice.FileId);
                invoice.FileMetadata = fileMetadata;
            }
        }

        return result;
    }

    public async Task<TransactionListDto> GetTransactionsForUserBetweenDates(GetTransactionsForUserBetweenDatesDto dto)
    {
        var transactions = await _transactionRepository
            .GetTransactionsForUserBetweenDates(dto.UserId, dto.DateFrom, dto.DateTo)
            .ConfigureAwait(false);
        
        var result = _mapper.Map<ICollection<Transaction>, TransactionListDto>(transactions);
        
        foreach (var transaction in result.Items)
        {
            foreach (var invoice in transaction.Invoices.Items)
            {
                var fileMetadata = await _fileClient.GetFileMetadataAsync(invoice.FileId);
                invoice.FileMetadata = fileMetadata;
            }
        }
        
        return result;
    }

    private TransactionOwnedByUserOrHisFamilyArgs GenerateDefaultPermissionCheckArgs(ulong transactionId)
    {
        return new TransactionOwnedByUserOrHisFamilyArgs
        {
            TransactionId = Convert.ToInt64(transactionId),
            UserClient = _userClient,
            FamilyClient = _familyClient
        };
    }
}