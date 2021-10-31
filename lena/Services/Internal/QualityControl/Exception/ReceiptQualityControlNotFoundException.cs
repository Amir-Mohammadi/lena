using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class ReceiptQualityControlNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ReceiptQualityControlNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
