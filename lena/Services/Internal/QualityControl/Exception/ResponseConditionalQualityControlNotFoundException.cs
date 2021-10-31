using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class ResponseConditionalQualityControlNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ResponseConditionalQualityControlNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
