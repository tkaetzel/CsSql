using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsSql.CoreTests;

namespace CsSql.Core.Tests
{
  [TestClass()]
  public class SqlOperationTests
  {
    [TestMethod()]
    public void ExecuteTest()
    {
      var db = new MockDb();
      db.Record.json = "{}";
      var op = new DbOperation(string.Empty, string.Empty);
      op.Transforms.Add(new JsonTransformation("json", "json.test = 123;", format: false));
      op.Execute(db);
      Assert.AreEqual(db.Record.json, "{\"test\":123}");
    }
  }
}