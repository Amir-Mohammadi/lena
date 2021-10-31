using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public QualityAssurance(IRepository repository)
    {
      this.repository = repository;
    }

  }
}