using System.Text.Json;
using CodingAssignmentLib.Abstractions;
using Newtonsoft.Json;

namespace CodingAssignmentLib;

public class JsonContentParser: IContentParser
{
    public IEnumerable<Data> Parse(string content)
    {
        var data = JsonConvert.DeserializeObject<List<Data>>(content);
        foreach (var item in data!)
        {
            yield return new Data
            (
                item.Key ?? "",
                item.Value ?? ""
            );
        }
    }
}