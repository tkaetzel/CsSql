using System.Collections.Generic;
using System.Data;

namespace CsSql.Core
{
  public class DbOperation
  {
    public DbOperation(string selectStatement, string updateStatement)
    {
      _selectStatement = selectStatement;
      _updateStatement = updateStatement;
      Transforms = new List<JsonTransformation>();
    }

    private readonly string _selectStatement;
    private readonly string _updateStatement;
    public readonly IList<JsonTransformation> Transforms;


    public void Execute(IDb db)
    {
      foreach (var record in db.Read(_selectStatement))
      {
        ExecuteScripts(record);
        db.Write(_updateStatement, (object)record);
      }
    }

    private void ExecuteScripts(dynamic record)
    {
      var recordDictionary = (IDictionary<string, object>)record;
      foreach (var transform in Transforms)
      {
        var originalJson = recordDictionary[transform.Field] as string;
        recordDictionary[transform.Field] = transform.Apply(originalJson,record);
      }
    }
  }
}
