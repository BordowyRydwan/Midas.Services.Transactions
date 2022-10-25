using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;

    public InvoiceService(IInvoiceRepository invoiceRepository, IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _mapper = mapper;
    }
    
    public async Task DeleteInvoice(Guid invoiceId)
    {
        await _invoiceRepository.DeleteInvoice(invoiceId).ConfigureAwait(false);
    }

    public async Task AddInvoice(AddInvoiceDto dto)
    {
        var invoice = _mapper.Map<AddInvoiceDto, Invoice>(dto);
        await _invoiceRepository.AddInvoice(invoice).ConfigureAwait(false);
    }
}