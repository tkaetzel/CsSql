using System.IO;
using System.Text;

namespace CsSql.Core
{
  internal static class CodeOperations
  {
    public static void AppendIndented(this StringBuilder builder, string indent, string text)
    {
      using (var reader = new StringReader(text ?? string.Empty))
      {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
          builder.Append(indent);
          builder.AppendLine(line.Trim());
        }
      }
    }
  }
}
