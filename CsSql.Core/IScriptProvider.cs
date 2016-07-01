using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsSql.Core
{
  public interface IScriptProvider
  {
    DbOperation Read(string text);
  }
}
