using Application.Dto;

namespace Application.Interfaces;

public interface IInvoiceService
{
    public Task DeleteInvoice(Guid invoiceId);
    public Task AddInvoice(AddInvoiceDto dto);
}