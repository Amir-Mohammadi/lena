using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class ResponseStuffRequestStuffIsNullException : InternalServiceException
  {
    public int? StuffId { get; }

    public ResponseStuffRequestStuffIsNullException(int? stuffId)
    {
      this.StuffId = stuffId;
    }
  }
}
