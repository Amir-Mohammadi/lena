using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public ApplicationBase(IRepository repository)
    {
      this.repository = repository;
    }
  }
}
