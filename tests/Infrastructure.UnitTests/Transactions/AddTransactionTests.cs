using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using MockQueryable.Moq;
using Moq;

namespace Infrastructure.UnitTests.Transactions;

[TestFixture]
public class AddTransactionTests
{
    private readonly ITransactionRepository _repository;

    private readonly IList<Transaction> _data = new List<Transaction>();

    public AddTransactionTests()
    {
        var mockContext = new Mock<TransactionDbContext>();
        var mockData = _data.AsQueryable().BuildMockDbSet();
        
        mockContext.Setup(x => x.Transactions).Returns(mockData.Object);
        mockData.Setup(x => x.AddAsync(It.IsAny<Transaction>(), default))
            .Callback<Transaction, CancellationToken>((i, _) => _data.Add(i));
        
        _repository = new TransactionRepository(mockContext.Object);
    }

    [TearDown]
    public void CleanInvoices()
    {
        _data.Clear();
    }

    [Test]
    public async Task ShouldAddTransaction_ForProperModel()
    {
        var transaction = new Transaction
        {
            Id = 1ul,
            Amount = 21.37m,
            Currency = new Currency
            {
                Code = "PLN",
                FactorToDefaultCurrency = 1,
                IsDefault = true
            },
            DateCreated = DateTime.UtcNow,
            Title = "Test title",
            Description = "Test description",
            RecipientName = "Test name",
            Invoices = new List<Invoice>
            {
                new()
                {
                    FileId = Guid.NewGuid()
                }
            }
        };

        await _repository.AddTransaction(transaction).ConfigureAwait(false);
        Assert.That(_data, Has.Count.EqualTo(1));
    }
    
    [Test]
    public async Task ShouldThrowNullArgumentException_ForNullishModel()
    {
        var exception = Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddTransaction(null));
;
        Assert.Multiple(() =>
        {
            Assert.That(_data, Has.Count.EqualTo(0));
            Assert.That(exception.ParamName, Is.EqualTo("Transaction argument is null"));
        });
    }
}