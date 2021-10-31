using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement
{
  public partial class ScrumManagement : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public ScrumManagement(IRepository repository)
    {
      this.repository = repository;
    }
  }
}
