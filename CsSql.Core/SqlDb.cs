using System.Collections.Generic;
using System.Data;
using Dapper;

namespace CsSql.Core
{
  public class SqlDb : IDb
  {
    private SqlDb(IDbConnection conn)
    {
      _conn = conn;
    }

    private readonly IDbConnection _conn;

    public IEnumerable<dynamic> Read(string query)
    {
      return _conn.Query(query);
    }

    public void Write(string query, object parameters)
    {
      _conn.Execute(query, parameters);
    }
    public static IDb From(IDbConnection conn) => new SqlDb(conn);
  }
}
