using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.Logger
{
  public partial class Logger : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public Logger(IRepository repository)
    {
      this.repository = repository;
    }
  }
}