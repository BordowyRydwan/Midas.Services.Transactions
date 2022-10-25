using System.Runtime.Serialization;
using Application.Dto;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.UnitTests.Mappings;

[TestFixture]
public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _mapper = AutoMapperConfig.Initialize();
        _configuration = _mapper.ConfigurationProvider;
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(Transaction), typeof(TransactionDto))]
    [TestCase(typeof(List<TransactionDto>), typeof(TransactionListDto))]
    public void ShouldSupportMappingMessageObjects(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);
        Assert.DoesNotThrow(() => _mapper.Map(instance, source, destination));
    }

    private object GetInstanceOf(Type type)
    {
        return type.GetConstructor(Type.EmptyTypes) is not null
            ? Activator.CreateInstance(type)!
            : FormatterServices.GetUninitializedObject(type);
    }
}