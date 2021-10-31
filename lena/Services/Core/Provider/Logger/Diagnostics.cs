using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Provider.Logger
{
  public class Diagnostics
  {
    private string trace = "";
    public Diagnostics()
    {

    }

    public void Diagnose()
    {
      trace = Environment.StackTrace;
    }


    public string Print()
    {
      if (trace == "")
      {
        return "";
      }
      var items = trace.Split(Environment.NewLine.ToCharArray());
      var newItems = string.Join(Environment.NewLine, items.Where(a => a.Contains("Parlar")));
      return newItems.Trim();
    }
  }
}
