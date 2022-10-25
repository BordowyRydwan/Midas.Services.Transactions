namespace Application.Dto;

public class GetTransactionsForUserBetweenDatesDto
{
    public ulong UserId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}