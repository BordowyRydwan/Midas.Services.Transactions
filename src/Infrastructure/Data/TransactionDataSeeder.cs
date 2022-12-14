using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class TransactionDataSeeder
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>().HasData(
            new Currency { Code = "PLN", IsDefault = true }
        );
        
        modelBuilder.Entity<TransactionCategory>().HasData(
            #region EXPENSES
            new TransactionCategory { Id = 1UL, Name = "Opłaty", IsIncome = false },
            new TransactionCategory { Id = 2UL, Name = "Subskrypcje", IsIncome = false },
            new TransactionCategory { Id = 3UL, Name = "Pożyczki / kredyty", IsIncome = false },
            new TransactionCategory { Id = 4UL, Name = "Podatki", IsIncome = false },
            new TransactionCategory { Id = 5UL, Name = "Transport", IsIncome = false },
            new TransactionCategory { Id = 6UL, Name = "Dom", IsIncome = false },
            new TransactionCategory { Id = 7UL, Name = "Zdrowie i uroda", IsIncome = false },
            new TransactionCategory { Id = 8UL, Name = "Produkty spożywcze", IsIncome = false },
            new TransactionCategory { Id = 9UL, Name = "Rozrywka", IsIncome = false },
            new TransactionCategory { Id = 10UL, Name = "Alimenty", IsIncome = false },
            new TransactionCategory { Id = 11UL, Name = "Dotacje", IsIncome = false },
            new TransactionCategory { Id = 12UL, Name = "Inwestycje", IsIncome = false },
            new TransactionCategory { Id = 13UL, Name = "Inne wydatki", IsIncome = false },
            #endregion
            
            #region INCOMES
            new TransactionCategory { Id = 14UL, Name = "Dotacje", IsIncome = true },
            new TransactionCategory { Id = 15UL, Name = "Wynagrodzenie", IsIncome = true },
            new TransactionCategory { Id = 16UL, Name = "Emerytura / renta", IsIncome = true },
            new TransactionCategory { Id = 17UL, Name = "Inwestycje", IsIncome = true },
            new TransactionCategory { Id = 18UL, Name = "Biznes", IsIncome = true },
            new TransactionCategory { Id = 19UL, Name = "Alimenty", IsIncome = true },
            new TransactionCategory { Id = 20UL, Name = "Inne przychody", IsIncome = true }
            #endregion
        );
    }
}