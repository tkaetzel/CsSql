using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CsSql.Core
{
  public class JsonTransformation
  {
    public JsonTransformation(string field, string code, bool format = true)
    {
      Field = field;
      _code = code;
      _executor = new Lazy<Action<object, object>>(Compile);
      _format = format;
    }

    public readonly string Field;
    private readonly bool _format;
    private readonly string _code;
    private readonly Lazy<Action<object, object>> _executor;

    public string Apply(string json, dynamic record)
    {
      var jsonObject = JsonConvert.DeserializeObject<dynamic>(json);
      _executor.Value.Invoke(jsonObject, record);
      return JsonConvert.SerializeObject(jsonObject, _format ? Formatting.Indented : Formatting.None);
    }

    private Action<object, object> Compile()
    {
      var compiler = new CSharpCompiler(Field);
      return compiler.Compile(_code);
    }
  }
}
