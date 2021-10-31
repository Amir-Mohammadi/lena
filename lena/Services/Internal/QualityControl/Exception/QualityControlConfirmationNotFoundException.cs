using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlConfirmationNotFoundException : InternalServiceException
  {
    public int? Id { get; }
    public int? QualityControlId { get; }

    public QualityControlConfirmationNotFoundException(int? id = null, int? qualityControlId = null)
    {
      this.Id = id;
      this.QualityControlId = qualityControlId;
    }
  }
}
