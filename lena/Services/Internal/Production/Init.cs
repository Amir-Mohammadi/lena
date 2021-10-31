using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public Production(IRepository repository)
    {
      this.repository = repository;
    }

  }
}
