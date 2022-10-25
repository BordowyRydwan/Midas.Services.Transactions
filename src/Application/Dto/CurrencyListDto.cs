using Application.Interfaces;

namespace Application.Dto;

public class CurrencyListDto : IListDto<CurrencyDto>
{
    public int Count { get; set; }
    public ICollection<CurrencyDto> Items { get; set; }
}