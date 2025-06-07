using System.IO.Abstractions;

namespace CodingAssignmentLib.Abstractions;


public abstract class ContentParserBase : IContentParser
{
    public abstract IEnumerable<Data> Parse(string content);
    
    public string Search(string file, string inputKey)
    {
        var fileUtility = new FileUtility(new FileSystem());
        var content = fileUtility.GetContent(file);
        var dataList = Parse(content);
        var foundData = dataList.FirstOrDefault(data =>
            !string.IsNullOrEmpty(data.Key) &&
            string.Equals(data.Key, inputKey, StringComparison.OrdinalIgnoreCase));

        if (string.IsNullOrEmpty(foundData.Key))
        {
            return "";
        }
        
        var marker = "data" + Path.DirectorySeparatorChar; 
        var index = file.LastIndexOf(marker, StringComparison.OrdinalIgnoreCase);
        var relativePath = file.Substring(index);
        return $"Key:{foundData.Key} Value:{foundData.Value} FileName:{relativePath}";
    }
}
public interface IContentParser
{
    IEnumerable<Data> Parse(string content);
}