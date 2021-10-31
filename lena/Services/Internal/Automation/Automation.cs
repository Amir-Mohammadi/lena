using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
using core.Data;
namespace lena.Services.Internals
{
  public partial class Automation : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public Automation(IRepository repository)
    {
      this.repository = repository;
    }
    private Automation()
    {
    }
  }
}