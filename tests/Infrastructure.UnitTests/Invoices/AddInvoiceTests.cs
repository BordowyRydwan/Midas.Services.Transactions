using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using MockQueryable.Moq;
using Moq;

namespace Infrastructure.UnitTests.Invoices;

[TestFixture]
public class AddInvoiceTests
{
    private readonly IInvoiceRepository _repository;

    private readonly IList<Transaction> _data = new List<Transaction>
    {
        new()
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
        }
    };

    public AddInvoiceTests()
    {
        var mockContext = new Mock<TransactionDbContext>();
        var mockData = _data.AsQueryable().BuildMockDbSet();
        
        mockContext.Setup(x => x.Transactions).Returns(mockData.Object);
        _repository = new InvoiceRepository(mockContext.Object);
    }

    [TearDown]
    public void CleanInvoices()
    {
        _data.First().Invoices = new List<Invoice> { new() { FileId = Guid.NewGuid() } };
    }

    [Test]
    public async Task ShouldAddInvoice_ForExistingTransaction()
    {
        var invoice = new Invoice { FileId = Guid.NewGuid(), TransactionId = 1ul};

        await _repository.AddInvoice(invoice);

        Assert.Multiple(() =>
        {
            Assert.That(_data.First(x => x.Id == invoice.TransactionId).DateModified, Is.Not.EqualTo(default(DateTime)));
            Assert.That(_data.First(x => x.Id == invoice.TransactionId).Invoices, Has.Count.EqualTo(2));
        });
    }
    
    [Test]
    public async Task ShouldThrowNullReferenceException_ForNonExistingTransaction()
    {
        var invoice = new Invoice { FileId = Guid.NewGuid(), TransactionId = 2137ul};
        var exception = Assert.ThrowsAsync<NullReferenceException>(
            () => _repository.AddInvoice(invoice)
        );
        
        Assert.That(exception.Message, Is.EqualTo($"Transaction with ID: {invoice.TransactionId} does not exist"));
    }
    
    [Test]
    public async Task ShouldThrowArgumentNullException_ForNullishInvoice()
    {
        var exception = Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddInvoice(null));
        
        Assert.That(exception.ParamName, Is.EqualTo("Invoice argument is null"));
    }
    
    [Test]
    public async Task ShouldThrowArgumentException_ForRepeatingInvoice()
    {
        var invoice = new Invoice { FileId = _data.First().Invoices.First().FileId, TransactionId = 1ul};
        var transactionId = 1ul;
        var exception = Assert.ThrowsAsync<ArgumentException>(
            () => _repository.AddInvoice(invoice)
        );

        Assert.Multiple(() =>
        {
            Assert.That(_data.First(x => x.Id == transactionId).Invoices, Has.Count.EqualTo(1));
            Assert.That(exception.Message, Is.EqualTo("File is already present in database"));
        });
    }
}