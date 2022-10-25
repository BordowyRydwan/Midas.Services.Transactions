using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using MockQueryable.Moq;
using Moq;

namespace Infrastructure.UnitTests.Transactions;

[TestFixture]
public class DeleteTransactionTests
{
    private readonly ITransactionRepository _repository;

    private readonly Transaction _mockObject = new()
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

    private readonly IList<Transaction> _data = new List<Transaction>();

    public DeleteTransactionTests()
    {
        _data.Add(_mockObject);
        
        var mockContext = new Mock<TransactionDbContext>();
        var mockData = _data.AsQueryable().BuildMockDbSet();
        
        mockContext.Setup(x => x.Transactions).Returns(mockData.Object);
        mockData.Setup(x => x.Remove(It.IsAny<Transaction>()))
            .Callback<Transaction>(i => _data.Remove(i));
        
        _repository = new TransactionRepository(mockContext.Object);
    }

    [TearDown]
    public void CleanInvoices()
    {
        _data.Clear();
        _data.Add(_mockObject);
    }

    [Test]
    public async Task ShouldDeleteTransaction_ForExisitingTransaction()
    {
        var transactionId = 1ul;

        await _repository.DeleteTransaction(transactionId).ConfigureAwait(false);
        Assert.That(_data, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task ShouldThrowNullReferenceException_ForNonExisitingTransaction()
    {
        var transactionId = 2137ul;
        var exception = Assert.ThrowsAsync<NullReferenceException>(() => _repository.DeleteTransaction(transactionId));

        Assert.Multiple(() =>
        {
            Assert.That(_data, Has.Count.EqualTo(1));
            Assert.That(exception.Message, Is.EqualTo($"Transaction with ID: {transactionId} does not exist"));
        });
    }
}