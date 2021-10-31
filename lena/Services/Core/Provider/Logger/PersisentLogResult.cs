using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Provider.Logger
{
  public class PersisentLogResult
  {
    public bool success { get; set; }

    public string cause { get; set; }

    public int? track { get; set; }
  }
}
