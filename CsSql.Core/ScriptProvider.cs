using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsSql.Core
{
  public static class ScriptProvider
  {
    static IScriptProvider FromFilename(string filename)
    {
      var extension = new FileInfo(filename).Extension;
      switch (extension?.ToLower())
      {
        case ".yaml":
          return new YamlScriptProvider();
        case ".xml":
          return new XmlScriptProvider();
        default:
          return null;
      }
    }

    public static DbOperation ReadFile(string filename)
    {
      var provider = FromFilename(filename);
      return provider?.Read(File.ReadAllText(filename));
    }
  }
}
