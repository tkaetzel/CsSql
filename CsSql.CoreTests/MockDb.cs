using System.Collections.Generic;
using System.Dynamic;
using CsSql.Core;

namespace CsSql.CoreTests
{
  internal class MockDb : IDb
  {
    public MockDb()
    {
      Record = new ExpandoObject();
    }

    public dynamic Record;

    public IEnumerable<dynamic> Read(string query)
    {
      return new[] { Record };
    }

    public void Write(string query, object parameters)
    {
      Record = parameters;
    }
  }
}
