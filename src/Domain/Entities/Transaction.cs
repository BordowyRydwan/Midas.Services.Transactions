namespace Domain.Entities;

public class Transaction
{
    public ulong Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string RecipientName { get; set; }
    public decimal Amount { get; set; }
    public ulong UserId { get; set; }
    
    public string CurrencyCode { get; set; }
    public Currency Currency { get; set; }
    
    public ulong TransactionCategoryId { get; set; }
    public TransactionCategory TransactionCategory { get; set; }

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}