using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class AssetTransferRequestNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public AssetTransferRequestNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}