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
            new TransactionCategory { Id = 1UL, Name = "Rent", IsIncome = false },
            new TransactionCategory { Id = 2UL, Name = "Subscriptions", IsIncome = false },
            new TransactionCategory { Id = 3UL, Name = "Mortgages", IsIncome = false },
            new TransactionCategory { Id = 4UL, Name = "Tax", IsIncome = false },
            new TransactionCategory { Id = 5UL, Name = "Transport", IsIncome = false },
            new TransactionCategory { Id = 6UL, Name = "Home", IsIncome = false },
            new TransactionCategory { Id = 7UL, Name = "Health & Beauty", IsIncome = false },
            new TransactionCategory { Id = 8UL, Name = "Food", IsIncome = false },
            new TransactionCategory { Id = 9UL, Name = "Entertainment", IsIncome = false },
            new TransactionCategory { Id = 10UL, Name = "Alimony", IsIncome = false },
            new TransactionCategory { Id = 11UL, Name = "Donation", IsIncome = false },
            new TransactionCategory { Id = 12UL, Name = "Investments", IsIncome = false },
            new TransactionCategory { Id = 13UL, Name = "Other expenses", IsIncome = false },
            #endregion
            
            #region INCOMES
            new TransactionCategory { Id = 14UL, Name = "Donation", IsIncome = true },
            new TransactionCategory { Id = 15UL, Name = "Salary", IsIncome = true },
            new TransactionCategory { Id = 16UL, Name = "Pension", IsIncome = true },
            new TransactionCategory { Id = 17UL, Name = "Investments", IsIncome = true },
            new TransactionCategory { Id = 18UL, Name = "Business", IsIncome = true },
            new TransactionCategory { Id = 19UL, Name = "Alimony", IsIncome = true },
            new TransactionCategory { Id = 20UL, Name = "Other incomes", IsIncome = true }
            #endregion
        );
    }
}