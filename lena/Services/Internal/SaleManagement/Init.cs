using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public SaleManagement(IRepository repository)
    {
      this.repository = repository;
    }
  }
}