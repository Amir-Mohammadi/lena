using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.ReportManagement
{
  public partial class ReportManagement : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public ReportManagement(IRepository repository)
    {
      this.repository = repository;
    }
  }
}
