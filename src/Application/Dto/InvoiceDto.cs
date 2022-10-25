using Midas.Services.FileStorage;

namespace Application.Dto;

public class InvoiceDto
{
    public Guid FileId { get; set; }
    public FileMetadataDto FileMetadata { get; set; }
}