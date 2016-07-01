using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsSql.CoreTests;

namespace CsSql.Core.Tests
{
  [TestClass()]
  public class XmlScriptProviderTests
  {
    [TestMethod()]
    public void ReadTest()
    {
      var db = new MockDb();
      db.Record.json = "{}";
      var xml = @"
<query>
  <select></select>
  <transforms>
    <transform field=""json"" format=""false"">
      json.test = 123;
    </transform>
  </transforms>
  <update></update>
</query>";
      var op = new XmlScriptProvider().Read(xml);
      op.Execute(db);
      Assert.AreEqual(db.Record.json, "{\"test\":123}");
    }
  }
}