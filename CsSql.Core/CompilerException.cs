using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Text;

namespace CsSql.Core
{
  public class CompilerException : Exception
  {
    public CompilerException(CompilerErrorCollection errors) : base("Unable to compile")
    {
      Errors = errors;
    }

    public readonly CompilerErrorCollection Errors;

    public string CompilerErrors()
    {
      var text = new StringBuilder();
      foreach (
        var error in
        from error in Errors.Cast<CompilerError>()
        select error)
      {
        text.AppendLine($"{error.Line}: {error.ErrorText}");
      }
      return text.ToString();
    }
  }
}
