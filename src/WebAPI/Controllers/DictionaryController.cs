using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers;

public class DictionaryController : ControllerBase
{
    private readonly IDictionaryService _dictionaryService;

    public DictionaryController(IDictionaryService dictionaryService)
    {
        _dictionaryService = dictionaryService;
    }

    [SwaggerOperation(Summary = "Get all possible currencies")]
    [ProducesResponseType(typeof(CurrencyListDto), 200)]
    [HttpGet("Currencies", Name = nameof(GetCurrencies))]
    public async Task<IActionResult> GetCurrencies()
    {
        var result = await _dictionaryService.GetCurrencies().ConfigureAwait(false);
        return Ok(result);
    }
    
    [SwaggerOperation(Summary = "Get all possible categories")]
    [ProducesResponseType(typeof(TransactionCategoryListDto), 200)]
    [HttpGet("TransactionCategories", Name = nameof(GetTransactionCategories))]
    public async Task<IActionResult> GetTransactionCategories()
    {
        var result = await _dictionaryService.GetTransactionCategories().ConfigureAwait(false);
        return Ok(result);
    }
}