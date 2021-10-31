using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public Supplies(IRepository repository)
    {
      this.repository = repository;
    }
  }
}