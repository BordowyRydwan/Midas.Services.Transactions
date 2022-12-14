using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly TransactionDbContext _context;
    
    public InvoiceRepository(TransactionDbContext context)
    {
        _context = context;
    }

    public async Task DeleteInvoice(Guid invoiceId)
    {
        var invoice = await _context.Invoices.FindAsync(invoiceId).ConfigureAwait(false);
        
        if (invoice is null)
        {
            throw new NullReferenceException($"Invoice with ID: {invoiceId} does not exist");
        }
        
        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
    
    public async Task AddInvoice(Invoice invoice)
    {
        if (invoice is null)
        {
            throw new ArgumentNullException("Invoice argument is null");
        }
        
        var transaction = await _context.Transactions
            .Include(x => x.Invoices)
            .SingleOrDefaultAsync(x => x.Id == invoice.TransactionId)
            .ConfigureAwait(false);

        if (transaction is null)
        {
            throw new NullReferenceException($"Transaction with ID: {invoice.TransactionId} does not exist");
        }

        if (transaction.Invoices is null)
        {
            transaction.Invoices = new List<Invoice>();
        }
        
        var isInvoiceAlreadyInList = transaction.Invoices.Any(x => x.FileId == invoice.FileId);

        if (isInvoiceAlreadyInList)
        {
            throw new ArgumentException("File is already present in database");
        }

        transaction.DateModified = DateTime.UtcNow;
        
        _context.Entry(invoice).State = EntityState.Added;
        transaction.Invoices.Add(invoice);
        
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}