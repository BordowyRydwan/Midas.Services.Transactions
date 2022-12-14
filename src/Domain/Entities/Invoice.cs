using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Invoice
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid FileId { get; set; }
    
    public ulong TransactionId { get; set; }
    public Transaction Transaction { get; set; }
}