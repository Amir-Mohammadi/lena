using lena.Services.Core.Foundation;
using lena.Domains.Enums;
using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public UserManagement(IRepository repository)
    {
      this.repository = repository;
    }
  }
}