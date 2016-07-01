using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsSql.Core.Tests
{
  [TestClass()]
  public class JsonScriptTests
  {
    [TestMethod()]
    public void ApplyTest()
    {
      const string original = "{\"x\":\"originalValue\"}";
      const string column = "json";
      const string code = "json.x = \"modifiedValue\";";
      
      var jsonScript = new JsonTransformation(column, code, format: false);
      var result = jsonScript.Apply(original, null);
      Assert.AreEqual("{\"x\":\"modifiedValue\"}", result);
    }

    [TestMethod()]
    public void RemovePropertyTest()
    {
      const string original = "{\"x\":\"originalValue\"}";
      const string column = "json";
      const string code = "((JObject)json).Remove(\"x\");";

      var jsonScript = new JsonTransformation(column, code, format: false);
      var result = jsonScript.Apply(original, null);
      Assert.AreEqual("{}", result);
    }
  }
}