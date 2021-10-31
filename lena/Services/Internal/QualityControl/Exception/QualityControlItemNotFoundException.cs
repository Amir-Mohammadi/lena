using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlItemNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public string Serial { get; }
    public int QualityControlId { get; }

    public QualityControlItemNotFoundException(
        int qualityControlId,
        string serial)
    {
      this.QualityControlId = qualityControlId;
      this.Serial = serial;
    }

    public QualityControlItemNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
