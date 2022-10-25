using Application.Interfaces;

namespace Application.Dto;

public class TransactionCategoryListDto : IListDto<TransactionCategoryDto>
{
    public int Count { get; set; }
    public ICollection<TransactionCategoryDto> Items { get; set; }
}