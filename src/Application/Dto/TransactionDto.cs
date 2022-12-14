namespace Application.Dto;

[Serializable]
public class TransactionDto
{
    public ulong Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string RecipientName { get; set; }
    public decimal Amount { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    public ulong UserId { get; set; }
    
    public CurrencyDto Currency { get; set; }
    public TransactionCategoryDto TransactionCategory { get; set; }
    public InvoiceListDto Invoices { get; set; }

    public TransactionDto() { }
}