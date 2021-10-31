using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class NotDiffrenceInRequestException : InternalServiceException
  {
    public string AssetCode { get; set; }
    public NotDiffrenceInRequestException(string assetCode)
    {
      this.AssetCode = assetCode;
    }
  }
}