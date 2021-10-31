using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public Planning(IRepository repository)
    {
      this.repository = repository;
    }
  }
}