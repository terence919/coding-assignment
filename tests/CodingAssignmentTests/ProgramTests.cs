using Microsoft.VisualStudio.TestPlatform.TestHost;
using CodingAssignment;
using CodingAssignmentLib.Abstractions;

namespace CodingAssignmentTests;

public class ProgramTests
{
    private string _rootPath = "";
    
    [SetUp]
    public void Setup()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir != null && dir.GetFiles("CodingAssignmentTests.csproj").Length == 0)
        {
            dir = dir.Parent;
        }

        if (dir == null)
        {
            throw new Exception("Project root not found.");
        }
        
        _rootPath = dir.FullName;
    }
    [Test]
    public void GetAllDataList_ReturnsData()
    {
        var filePath = Path.Combine(_rootPath, "MockData", "data");
        var allDataList = CodingAssignment.Program.GetAllFilesName(filePath);
        Assert.That(allDataList.Count, Is.EqualTo(3));
    }
    
    [Test]
    public void GetAllDataList_Invalid_FilePath_Returns_EmptyList()
    {
        var filePath = Path.Combine(_rootPath, "MockData", "data2");
        var allDataList = CodingAssignment.Program.GetAllFilesName(filePath);
        Assert.That(allDataList.Count, Is.EqualTo(0));
    }
    
    [Test]
    public void DisplaySearchInfo_ReturnsInfo()
    {
        var filePath = Path.Combine(_rootPath, "MockData", "data");
        var allDataList = CodingAssignment.Program.GetAllFilesName(filePath);
        var searchJsonInfo = CodingAssignment.Program.DisplaySearchInfo("abc ", allDataList);
        Assert.That(searchJsonInfo, Is.EqualTo("Key:aBc Value:123 FileName:data/data2.json"));
        var searchCsvInfo = CodingAssignment.Program.DisplaySearchInfo("aaa ", allDataList);
        Assert.That(searchCsvInfo, Is.EqualTo("Key:aaa Value:bbb FileName:data/data2.csv"));
        var searchXmlInfo = CodingAssignment.Program.DisplaySearchInfo("hhh ", allDataList);
        Assert.That(searchXmlInfo, Is.EqualTo("Key:Hhh Value:555 FileName:data/data2.xml"));
    }
    
    [Test]
    public void DisplaySearchInfo_InvalidKey_ReturnsInfo()
    {
        var filePath = Path.Combine(_rootPath, "MockData", "data");
        var filesName = CodingAssignment.Program.GetAllFilesName(filePath);
        var searchInvalidKey = CodingAssignment.Program.DisplaySearchInfo("abcd", filesName);
        Assert.That(searchInvalidKey, Is.EqualTo("Search not found."));
        var searchEmptyInfo = CodingAssignment.Program.DisplaySearchInfo(" ", filesName);
        Assert.That(searchEmptyInfo, Is.EqualTo("Search not found."));
    }
    
    [Test]
    public void GetDataList_ReturnsInfo()
    {
        var filePath = Path.Combine(_rootPath, "MockData", "data");
        var fileName = Path.Combine(filePath, "data2.csv");
        var dataList = CodingAssignment.Program.GetDataList(fileName);
        Assert.That(dataList, Is.EqualTo(new List<Data>
        {
            new("aaa", "bbb"),
            new("ccc", "ddd")
        }).AsCollection);
        
        fileName = Path.Combine(filePath, "data2.json");
        dataList = CodingAssignment.Program.GetDataList(fileName);
        Assert.That(dataList, Is.EqualTo(new List<Data>
        {
            new("aBc", "123"),
            new("cDf", "456")
        }).AsCollection);
        
        fileName = Path.Combine(filePath, "data2.xml");
        dataList = CodingAssignment.Program.GetDataList(fileName);
        Assert.That(dataList, Is.EqualTo(new List<Data>
        {
            new("Kkk", "789"),
            new("Hhh", "555")
        }).AsCollection);
    }
    
    [Test]
    public void GetDataList_Invalid_File_Ext_Returns_Empty()
    {
        var filePath = Path.Combine(_rootPath, "MockData", "data");
        var fileName = Path.Combine(filePath, "data2.txt");
        var dataList = CodingAssignment.Program.GetDataList(fileName);
        Assert.That(dataList, Is.Empty);
    }
    
    [Test]
    public void GetDataList_File_Not_Found_Returns_Empty()
    {
        var filePath = Path.Combine(_rootPath, "MockData", "data");
        var fileName = Path.Combine(filePath, "data1.txt");
        var dataList = CodingAssignment.Program.GetDataList(fileName);
        Assert.That(dataList, Is.Empty);
    }
}