using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.EmployeeCartable
{
  public partial class EmployeeCartable : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public EmployeeCartable(IRepository repository)
    {
      this.repository = repository;
    }
  }
}