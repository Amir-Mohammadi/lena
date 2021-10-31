using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public Accounting(IRepository repository)
    {
      this.repository = repository;
    }
  }
}