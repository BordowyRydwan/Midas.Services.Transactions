using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Currency
{
    public string Code { get; set; }
    public bool IsDefault { get; set; }
    public decimal FactorToDefaultCurrency { get; set; } = 1;
}