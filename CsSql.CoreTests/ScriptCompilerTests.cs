using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Dynamic;

namespace CsSql.Core.Tests
{
  [TestClass()]
  public class ScriptCompilerTests
  {

    [TestMethod()]
    public void CompileTest()
    {
      var compiler = new CSharpCompiler("test");
      var func = compiler.Compile("test.prop2 = test.prop;");
      dynamic test = new ExpandoObject();
      test.prop = "test";
      func(test, null);
      Assert.AreEqual(test.prop, test.prop2);
    }

    [TestMethod()]
    public void CompileThrowsCompilerExceptionTest()
    {
      var compiler = new CSharpCompiler("test");
      try
      {
        var func = compiler.Compile("not valid c# code");
        func(null, null);
        Assert.Fail();
      }
      catch (Exception err)
      {
        Assert.IsInstanceOfType(err, typeof(CompilerException));
      }
    }

  }
}