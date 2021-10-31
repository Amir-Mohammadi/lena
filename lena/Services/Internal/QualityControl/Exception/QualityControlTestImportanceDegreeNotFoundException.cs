using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlTestImportanceDegreeNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public QualityControlTestImportanceDegreeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
