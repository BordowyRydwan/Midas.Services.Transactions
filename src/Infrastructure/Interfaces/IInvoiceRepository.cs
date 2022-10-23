using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IInvoiceRepository : IRepository
{
    public Task DeleteInvoice(Guid invoiceId);
    public Task AddInvoice(Invoice invoice);
}