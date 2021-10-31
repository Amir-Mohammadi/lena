using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement
{
  public partial class ProjectManagement : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public ProjectManagement(IRepository repository)
    {
      this.repository = repository;
    }
  }
}