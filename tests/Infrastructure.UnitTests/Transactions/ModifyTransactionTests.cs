using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using MockQueryable.Moq;
using Moq;

namespace Infrastructure.UnitTests.Transactions;

[TestFixture]
public class ModifyTransactionTests
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

    public ModifyTransactionTests()
    {
        _data.Add(_mockObject);
        
        var mockContext = new Mock<TransactionDbContext>();
        var mockData = _data.AsQueryable().BuildMockDbSet();
        
        mockContext.Setup(x => x.Transactions).Returns(mockData.Object);
        _repository = new TransactionRepository(mockContext.Object);
    }

    [TearDown]
    public void CleanInvoices()
    {
        _data.Clear();
        _data.Add(_mockObject);
    }

    [Test]
    public async Task ShouldModifyTransaction_ForExisitingTransaction()
    {
        var transaction = _mockObject;
        
        transaction.Title = "Some title";
        transaction.Description = "Some description";

        await _repository.ModifyTransaction(transaction).ConfigureAwait(false);
        
        Assert.Multiple(() =>
        {
            Assert.That(_data.First().Title, Is.EqualTo(transaction.Title));
            Assert.That(_data.First().Description, Is.EqualTo(transaction.Description));
        });
    }

    [Test]
    public async Task ShouldNotUpdateValues_ForNonExisitingTransaction()
    {
        var transaction = _mockObject;

        transaction.Id = 2137ul;
        transaction.Title = "Some title";
        transaction.Description = "Some description";

        Assert.Multiple(() =>
        {
            Assert.That(_data.First().Title, Is.EqualTo(_mockObject.Title));
            Assert.That(_data.First().Description, Is.EqualTo(_mockObject.Description));
        });
    }

}