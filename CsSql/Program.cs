using System.Data.SqlClient;
using System.Linq;
using CsSql.Core;

namespace CsSql
{
  class Program
  {
    static void Main(string[] args)
    {
      using (var conn = new SqlConnection(args[0]))
      {
        foreach (var filename in args.Skip(1))
        {
          ScriptProvider.ReadFile(filename).Execute(SqlDb.From(conn));
        }
      }
    }
  }
}
