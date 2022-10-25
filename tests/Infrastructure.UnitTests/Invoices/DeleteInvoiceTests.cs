using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using MockQueryable.Moq;
using Moq;

namespace Infrastructure.UnitTests.Invoices;

[TestFixture]
public class DeleteInvoiceTests
{
    private readonly IInvoiceRepository _repository;

    private IList<Invoice> _data = new List<Invoice>
    {
        new()
        {
            FileId = Guid.NewGuid()
        }
    };

    public DeleteInvoiceTests()
    {
        var mockContext = new Mock<TransactionDbContext>();
        var mockData = _data.AsQueryable().BuildMockDbSet();
        
        mockContext.Setup(x => x.Invoices).Returns(mockData.Object);
        mockData.Setup(x => x.FindAsync(It.IsAny<object>())).ReturnsAsync((object[] ids) =>
        {
            var guid = (Guid)ids[0];
            return _data.FirstOrDefault(x => x.FileId == guid);
        });
        mockData.Setup(x => x.Remove(It.IsAny<Invoice>())).Callback<Invoice>(i => _data.Remove(i));
        _repository = new InvoiceRepository(mockContext.Object);
    }

    [TearDown]
    public void CleanInvoices()
    {
        _data = new List<Invoice> { new() { FileId = Guid.NewGuid() } };
    }

    [Test]
    public async Task ShouldDelete_ForExistingInvoice()
    {
        var guid = _data.First().FileId;

        await _repository.DeleteInvoice(guid).ConfigureAwait(false);
        Assert.That(_data, Has.Count.EqualTo(0));
    }
    
    [Test]
    public async Task ShouldThrowNullReferenceException_ForNotExistingInvoice()
    {
        var guid = Guid.Empty;
        var exception = Assert.ThrowsAsync<NullReferenceException>(
            () => _repository.DeleteInvoice(guid)
        );
        
        Assert.Multiple(() =>
        {
            Assert.That(_data, Has.Count.EqualTo(1));
            Assert.That(exception.Message, Is.EqualTo($"Invoice with ID: {guid} does not exist"));
        });
    }
}