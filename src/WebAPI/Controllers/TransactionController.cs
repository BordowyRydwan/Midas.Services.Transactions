using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers;

public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService)
    {
        _logger = logger;
        _transactionService = transactionService;
    }

    [SwaggerOperation(Summary = "Add transaction")]
    [HttpPost("Add", Name = nameof(AddTransaction))]
    public async Task<IActionResult> AddTransaction(TransactionDto dto)
    {
        try
        {
            await _transactionService.AddTransaction(dto).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace);
            return BadRequest(ex.Message);
        }
    }
    
    [SwaggerOperation(Summary = "Delete transaction")]
    [HttpDelete("Delete", Name = nameof(DeleteTransaction))]
    public async Task<IActionResult> DeleteTransaction(ulong id)
    {
        try
        {
            await _transactionService.DeleteTransaction(id).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace);
            return BadRequest(ex.Message);
        }
    }
    
    [SwaggerOperation(Summary = "Modify transaction")]
    [HttpPatch("Modify", Name = nameof(ModifyTransaction))]
    public async Task<IActionResult> ModifyTransaction(TransactionDto dto)
    {
        try
        {
            await _transactionService.ModifyTransaction(dto).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace);
            return BadRequest(ex.Message);
        }
    }
    
    [SwaggerOperation(Summary = "Get all transactions for user")]
    [ProducesResponseType(typeof(TransactionListDto), 200)]
    [HttpGet("Transactions/{userId}", Name = nameof(GetTransactionsForUser))]
    public async Task<IActionResult> GetTransactionsForUser(ulong userId)
    {
        try
        {
            var result = await _transactionService.GetTransactionsForUser(userId).ConfigureAwait(false); 
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace);
            return BadRequest(ex.Message);
        }
    }
    
    [SwaggerOperation(Summary = "Get all transactions for user")]
    [ProducesResponseType(typeof(TransactionListDto), 200)]
    [HttpGet("Transactions/{userId}", Name = nameof(GetTransactionsForUserAndBetweenDates))]
    public async Task<IActionResult> GetTransactionsForUserAndBetweenDates(GetTransactionsForUserBetweenDatesDto dto)
    {
        try
        {
            var result = await _transactionService.GetTransactionsForUserBetweenDates(dto).ConfigureAwait(false); 
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace);
            return BadRequest(ex.Message);
        }
    }
}