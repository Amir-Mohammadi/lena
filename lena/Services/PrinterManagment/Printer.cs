using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.PrinterManagment
{
  public abstract class Printer : IPrinter
  {
    public virtual string Name { get; set; }
    public virtual string NetworkAddress { get; set; }

    public abstract void Print(string text);
  }
}
