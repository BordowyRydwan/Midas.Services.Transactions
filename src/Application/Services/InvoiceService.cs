using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Midas.Services.FileStorage;

namespace Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IFileStorageClient _fileStorage;
    private readonly IMapper _mapper;

    public InvoiceService(IInvoiceRepository invoiceRepository, IFileStorageClient fileStorage, IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _fileStorage = fileStorage;
        _mapper = mapper;
    }
    
    public async Task DeleteInvoice(Guid invoiceId)
    {
        await _invoiceRepository.DeleteInvoice(invoiceId).ConfigureAwait(false);
        await _fileStorage.MarkFileAsDeletedAsync(invoiceId).ConfigureAwait(false);
    }

    public async Task<FileMetadataDto> AddInvoice(AddInvoiceDto dto)
    {
        var invoice = _mapper.Map<AddInvoiceDto, Invoice>(dto);
        await _invoiceRepository.AddInvoice(invoice).ConfigureAwait(false);

        var result = await _fileStorage.GetFileMetadataAsync(dto.FileId).ConfigureAwait(false);
        return result;
    }
}