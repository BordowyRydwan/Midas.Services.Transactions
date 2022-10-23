namespace Application.Dto;

public class CurrencyDto
{
    public string Code { get; set; }
    public bool IsDefault { get; set; }
    public decimal FactorToDefaultCurrency { get; set; }
}