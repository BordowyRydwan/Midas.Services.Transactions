using Application.Interfaces;

namespace Application.Dto;

public class InvoiceListDto : IListDto<InvoiceDto>
{
    public int Count { get; set; }
    public ICollection<InvoiceDto> Items { get; set; }
}