using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplicationSettings
{
  public partial class ApplicationSetting : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public ApplicationSetting(IRepository repository)
    {
      this.repository = repository;
    }
  }
}
