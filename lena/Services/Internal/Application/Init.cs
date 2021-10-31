using lena.Services.Core.Foundation;
using System;

using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.Application
{
  public partial class ApplicationManagement : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public ApplicationManagement(IRepository repository)
    {
      this.repository = repository;
    }

  }
}
