using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlConfirmationItemNotFoundException : InternalServiceException
  {
    public long Id { get; }
    public int QualityControlConfirmationId { get; }
    public string Serial { get; }

    public QualityControlConfirmationItemNotFoundException(long id)
    {
      this.Id = id;
    }

    public QualityControlConfirmationItemNotFoundException(int qualityControlConfirmationId, string serial)
    {
      this.QualityControlConfirmationId = qualityControlConfirmationId;
      this.Serial = serial;
    }
  }
}
