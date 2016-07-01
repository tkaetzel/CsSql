using System.Collections.Generic;

namespace CsSql.Core
{
  public interface IDb
  {
    IEnumerable<dynamic> Read(string query);

    void Write(string query, object parameters);
  }
}
