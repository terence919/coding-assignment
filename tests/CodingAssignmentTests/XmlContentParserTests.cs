using CodingAssignmentLib;
using CodingAssignmentLib.Abstractions;

namespace CodingAssignmentTests;

public class XmlContentParserTests
{
    private XmlContentParser _sut = null!;
    
    [SetUp]
    public void Setup()
    {
        _sut = new XmlContentParser();
    }

    [Test]
    public void Parse_ReturnsData()
    {
        const string content = @"
        <Datas>
          <Data>
            <Key>123</Key>
            <Value>456</Value>
          </Data>
          <Data>
            <Key>789</Key>
            <Value>012</Value>
          </Data>
        </Datas>";
        var dataList = _sut.Parse(content).ToList();
        Assert.That(dataList, Is.EqualTo(new List<Data>
        {
            new("123", "456"),
            new("789", "012")
        }).AsCollection);
    }
    
    [Test]
    public void Parse_Returns_Empty_Invalid_Key_Value()
    {
        const string content = @"
        <Datas>
          <Data>
            <Keys>123</Keys>
            <Values>456</Values>
          </Data>
          <Data>
            <Key>789</Key>
            <Value>012</Value>
          </Data>
        </Datas>";
        var dataList = _sut.Parse(content).ToList();
        Assert.That(dataList, Is.EqualTo(new List<Data>
        {
            new("",""),
            new("789", "012")
        }).AsCollection);
    }
}