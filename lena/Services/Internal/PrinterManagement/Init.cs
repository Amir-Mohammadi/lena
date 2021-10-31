using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.PrinterManagement
{
  public partial class PrinterManagement : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public PrinterManagement(IRepository repository)
    {
      this.repository = repository;
    }

  }
}
