using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public QualityControl(IRepository repository)
    {
      this.repository = repository;
    }
  }
}