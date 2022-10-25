using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers;

public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    private readonly ILogger<InvoiceController> _logger;

    public InvoiceController(ILogger<InvoiceController> logger, IInvoiceService invoiceService)
    {
        _logger = logger;
        _invoiceService = invoiceService;
    }

    [SwaggerOperation(Summary = "Add invoice to transaction")]
    [HttpPost("Invoice", Name = nameof(AddInvoice))]
    public async Task<IActionResult> AddInvoice(AddInvoiceDto dto)
    {
        try
        {
            await _invoiceService.AddInvoice(dto).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace);
            return BadRequest(ex.Message);
        }
    }
    
    [SwaggerOperation(Summary = "Remove invoice from transaction")]
    [HttpDelete("Invoice", Name = nameof(DeleteInvoice))]
    public async Task<IActionResult> DeleteInvoice(Guid id)
    {
        try
        {
            await _invoiceService.DeleteInvoice(id).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace);
            return BadRequest(ex.Message);
        }
    }
}