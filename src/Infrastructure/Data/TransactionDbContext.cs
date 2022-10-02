using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data;

public class TransactionDbContext : DbContext
{
    public virtual DbSet<Currency> Currencies { get; set; }
    public virtual DbSet<Invoice> Invoices { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<TransactionCategory> TransactionCategories { get; set; }
    
    public TransactionDbContext() { }

    public TransactionDbContext(DbContextOptions options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>().HasKey(x => x.Code);
        modelBuilder.Entity<Invoice>().HasKey(x => x.FileId);
        modelBuilder.Seed();
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }
}