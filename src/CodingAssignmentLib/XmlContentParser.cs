using System.Xml.Linq;
using CodingAssignmentLib.Abstractions;

namespace CodingAssignmentLib;

public class XmlContentParser : ContentParserBase
{
    public override IEnumerable<Data> Parse(string content)
    {
        var doc = XDocument.Parse(content);
        return doc.Descendants("Data")
            .Select(item => new Data
            (
                item.Element("Key")?.Value ?? "",
                item.Element("Value")?.Value ?? ""
            ))
            .ToList();
    }
}
