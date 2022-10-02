namespace Domain.Entities;

public class TransactionCategory
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public bool IsIncome { get; set; }
}