using Application.Interfaces;

namespace Application.Dto;

public class TransactionListDto : IListDto<TransactionDto>
{
    public int Count { get; set; }
    public ICollection<TransactionDto> Items { get; set; }
}