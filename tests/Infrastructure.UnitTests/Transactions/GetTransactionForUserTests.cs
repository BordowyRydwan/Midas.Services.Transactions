using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using MockQueryable.Moq;
using Moq;

namespace Infrastructure.UnitTests.Transactions;

[TestFixture]
public class GetTransactionForUserTests
{
    private readonly ITransactionRepository _repository;

    private readonly IQueryable<Transaction> _data = new List<Transaction>
    {
        new() { Id = 1, UserId = 1 },
        new() { Id = 2, UserId = 2 },
        new() { Id = 3, UserId = 2 },
        new()
        {
            Id = 4,
            UserId = 3,
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
        },
    }.AsQueryable();

    public GetTransactionForUserTests()
    {
        var mockContext = new Mock<TransactionDbContext>();
        var mockData = _data.AsQueryable().BuildMockDbSet();

        mockData.Setup(x => x.FindAsync(It.IsAny<ulong>())).ReturnsAsync((object[] ids) =>
        {
            var id = (ulong)ids[0];
            return _data.FirstOrDefault(x => x.Id == id);
        });
        mockContext.Setup(x => x.Transactions).Returns(mockData.Object);

        _repository = new TransactionRepository(mockContext.Object);
    }

    [Test]
    [TestCase(1ul, 1)]
    [TestCase(2ul, 2)]
    [TestCase(3ul, 1)]
    [TestCase(2137ul, 0)]
    public async Task ShouldProperlyFilterByUser(ulong userId, int expectedCount)
    {
        var result = await _repository.GetTransactionsForUser(userId).ConfigureAwait(false);
        Assert.That(result, Has.Count.EqualTo(expectedCount));
    }
    
    [Test]
    [TestCase(3ul)]
    public async Task ShouldProperlyGetObjectReferences(ulong userId)
    {
        var result = await _repository.GetTransactionsForUser(userId).ConfigureAwait(false);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Currency, Is.Not.Null);
            Assert.That(result.First().Invoices, Is.Not.Empty);
        });
    }
}