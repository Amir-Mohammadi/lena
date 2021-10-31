using lena.Services.Core.Foundation;
using lena.Domains.Enums;
using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public WarehouseManagement(IRepository repository)
    {
      this.repository = repository;
    }
  }
}