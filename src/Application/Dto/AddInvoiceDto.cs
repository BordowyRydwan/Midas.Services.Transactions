namespace Application.Dto;

public class AddInvoiceDto
{
    public ulong TransactionId { get; set; }
    public Guid FileId { get; set; }
}