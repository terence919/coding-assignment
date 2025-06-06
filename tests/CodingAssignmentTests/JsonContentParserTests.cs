using CodingAssignmentLib;
using CodingAssignmentLib.Abstractions;

namespace CodingAssignmentTests;

public class JsonContentParserTests
{
    private JsonContentParser _sut = null!;
    
    [SetUp]
    public void Setup()
    {
        _sut = new JsonContentParser();
    }

    [Test]
    public void Parse_ReturnsData()
    {
        var content = "[{\"Key\":\"abc\",\"Value\":\"123\"},{\"Key\":\"cdf\",\"Value\":\"456\"}]";
        var dataList = _sut.Parse(content).ToList();
        Assert.That(dataList, Is.EqualTo(new List<Data>
        {
            new("abc", "123"),
            new("cdf", "456")
        }).AsCollection);
    }
    
    [Test]
    public void Parse_Returns_Empty_Invalid_Key_Value()
    {
        var content = "[{\"Key1\":\"abc\",\"Value1\":\"123\"},{\"Key\":\"cdf\",\"Value\":\"456\"}]";
        var dataList = _sut.Parse(content).ToList();
        Assert.That(dataList, Is.EqualTo(new List<Data>
        {
            new("",""),
            new("cdf", "456")
        }).AsCollection);
    }
}