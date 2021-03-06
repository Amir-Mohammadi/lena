using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class NewShoppingDetailSummaryForNewShoppingDetailNotFoundException : InternalServiceException
  {
    public int NewShoppingDetailId { get; }

    public NewShoppingDetailSummaryForNewShoppingDetailNotFoundException(int newShoppingDetailId)
    {
      this.NewShoppingDetailId = newShoppingDetailId;
    }
  }
}
