using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace CsSql.Core
{
  public class CSharpCompiler
  {
    public CSharpCompiler(string field)
    {
      _field = field;
      _codeProvider = new CSharpCodeProvider();
      _options = new CompilerParameters(new[] {
        "mscorlib.dll",
        "System.dll",
        "System.Core.dll",
        "Microsoft.CSharp.dll",
        "Newtonsoft.Json.dll"
      });
      _options.GenerateInMemory = true;
    }

    private readonly string _field;
    private readonly CodeDomProvider _codeProvider;
    private readonly CompilerParameters _options;

    private const string GeneratedClassName = "GeneratedScript";
    private const string GeneratedMethodName = "Execute";

    public Action<object, object> Compile(string script)
    {
      var code = WriteCSharpClass(script);
      var assembly = GenerateAssembly(code);
      var method = GetGeneratedMethod(assembly);
      var jsonParam = Expression.Parameter(typeof(object));
      var recordParam = Expression.Parameter(typeof(object));
      var call = Expression.Call(method, jsonParam, recordParam);
      var lambda = Expression.Lambda<Action<object, object>>(call, jsonParam, recordParam);
      return lambda.Compile();
    }

    private MethodInfo GetGeneratedMethod(Assembly assembly)
    {
      return (from type in assembly.GetTypes()
              where type.Name == GeneratedClassName
              from method in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
              where method.Name == GeneratedMethodName
              select method).First();
    }

    private Assembly GenerateAssembly(string cSharpCode)
    {
      var compiledCode = _codeProvider.CompileAssemblyFromSource(_options, cSharpCode);
      if (compiledCode.Errors.Count > 0)
      {
        throw new CompilerException(compiledCode.Errors);
      }
      return compiledCode.CompiledAssembly;
    }

    private string WriteCSharpClass(string script)
    {
      var code = new StringBuilder();
      code.AppendLine("using System;");
      code.AppendLine("using System.Collections.Generic;");
      code.AppendLine("using System.Linq;");
      code.AppendLine("using System.Text;");
      code.AppendLine("using Microsoft.CSharp;");
      code.AppendLine("using Newtonsoft.Json;");
      code.AppendLine("using Newtonsoft.Json.Linq;");
      code.AppendLine();
      code.AppendLine($"class {GeneratedClassName}");
      code.AppendLine("{");
      code.AppendLine($"  static void {GeneratedMethodName}(dynamic {_field}, dynamic record)");
      code.AppendLine("  {");
      code.AppendIndented("    ", script);
      code.AppendLine("  }");
      code.AppendLine("}");
      return code.ToString();
    }
  }
}
