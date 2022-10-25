using Application.Dto;
using Application.Interfaces;

namespace Application.UnitTests.Common;

[TestFixture]
public class ListDtoTests
{
    [Test]
    [TestCase(typeof(CurrencyListDto))]
    public void ShouldHaveCorrectListDtoProperties(Type dto)
    {
        Has.Property("Count").And.Property("Items");
    }

    [Test]
    [TestCase(typeof(CurrencyListDto), typeof(IListDto<CurrencyDto>))]
    public void ShouldDeriveFromIListDto(Type listDto, Type objectDto)
    {
        var result = objectDto.IsAssignableFrom(listDto);
        Assert.That(result, Is.True);
    }

    [Test]
    public void ShouldGettersAndSettersWorkCorrectly()
    {
        var obj = new CurrencyListDto();

        Assert.Multiple(() =>
        {
            Assert.That(0, Is.EqualTo(obj.Count));
            Assert.That(obj.Items, Is.Null);
        });
    }
}