using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsSql.CoreTests;

namespace CsSql.Core.Tests
{
  [TestClass()]
  public class YamlScriptProviderTests
  {
    [TestMethod()]
    public void ReadTest()
    {
      var db = new MockDb();
      db.Record.json = "{}";
      var yaml = @"
for: json
in: 
transform: |
  json.test = 123;
then: ";
      var op = new YamlScriptProvider().Read(yaml);
      op.Execute(db);
      Assert.AreEqual(db.Record.json, "{\"test\":123}");
    }
  }
}