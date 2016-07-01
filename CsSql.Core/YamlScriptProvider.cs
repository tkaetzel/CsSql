using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CsSql.Core
{
  public class YamlScriptProvider : IScriptProvider
  {
    public YamlScriptProvider()
    {
      _deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
    }

    private readonly Deserializer _deserializer;

    public DbOperation Read(string yaml)
    {
      using (var reader = new StringReader(yaml))
      {
        var script = _deserializer.Deserialize<Script>(reader);
        var operation = new DbOperation(script.In, script.Then);
        operation.Transforms.Add(new JsonTransformation(script.For, script.Transform, script.Format));
        return operation;
      }
    }

    public class Script
    {
      public bool Format { get; set; }
      public string For { get; set; }
      public string In { get; set; }
      public string Transform { get; set; }
      public string Then { get; set; }
    }
  }
}
