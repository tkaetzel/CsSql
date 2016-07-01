using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CsSql.Core
{
  public class XmlScriptProvider : IScriptProvider
  {
    public DbOperation Read(string xml)
    {
      var document = XDocument.Parse(xml);
      var select = document.Descendants("select").First().Value;
      var update = document.Descendants("update").First().Value;
      var operation = new DbOperation(select, update);
      foreach (
        var transform in
        from transform in document.Descendants("transforms").Descendants("transform")
        let field = transform.Attribute("field")?.Value ?? "json"
        let formatAttribute = transform.Attribute("format")?.Value
        let format = formatAttribute == null ? true : bool.Parse(formatAttribute)
        select new JsonTransformation(field, transform.Value, format))
      {
        operation.Transforms.Add(transform);
      }
      return operation;
    }
  }
}
