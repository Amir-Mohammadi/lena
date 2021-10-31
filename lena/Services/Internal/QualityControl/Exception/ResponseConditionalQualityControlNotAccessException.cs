using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class ResponseConditionalQualityControlNotAccessException : InternalServiceException
  {
    public int Id { get; }

    public ResponseConditionalQualityControlNotAccessException(int id)
    {
      this.Id = id;
    }
  }
}
