using Application.Dto;
using Midas.Services.FileStorage;

namespace Application.Interfaces;

public interface IInvoiceService
{
    public Task DeleteInvoice(Guid invoiceId);
    public Task<FileMetadataDto> AddInvoice(AddInvoiceDto dto);
}