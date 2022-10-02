namespace Domain.Entities;

public class Invoice
{
    public Guid FileId { get; set; }
    
    public ulong TransactionId { get; set; }
    public Transaction Transaction { get; set; }
}