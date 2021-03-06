using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseOrderDetailSummaryNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public PurchaseOrderDetailSummaryNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
