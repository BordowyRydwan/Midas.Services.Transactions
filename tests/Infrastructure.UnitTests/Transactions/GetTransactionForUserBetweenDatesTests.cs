using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using MockQueryable.Moq;
using Moq;

namespace Infrastructure.UnitTests.Transactions;

[TestFixture]
public class GetTransactionForUserBetweenDatesTests
{
    private readonly ITransactionRepository _repository;
    
    private readonly IQueryable<Transaction> _data = new List<Transaction>
    {
        new() { Id = 1, UserId = 1, DateCreated = new DateTime(2022, 6, 9)},
        new() { Id = 2, UserId = 1, DateCreated = new DateTime(2022, 5, 9) },
        new() { Id = 3, UserId = 1, DateCreated = new DateTime(2022, 7, 9) },
        new() { Id = 4, UserId = 2, DateCreated = new DateTime(2022, 5, 9) },
        new() { Id = 5, UserId = 2, DateCreated = new DateTime(2022, 6, 9) },
    }.AsQueryable();

    public GetTransactionForUserBetweenDatesTests()
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
    [TestCase(2ul, 1)]
    [TestCase(2137ul, 0)]
    public async Task ShouldProperlyFilterByUserAndDate(ulong userId, int expectedCount)
    {
        var dateFrom = new DateTime(2022, 6, 1);
        var dateTo = new DateTime(2022, 6, 30);
        
        var result = await _repository.GetTransactionsForUserBetweenDates(userId, dateFrom, dateTo).ConfigureAwait(false);
        Assert.That(result, Has.Count.EqualTo(expectedCount));
    }
}