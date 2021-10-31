using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class PurchaseOrderCostNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public PurchaseOrderCostNotFoundException(int id)
    {
      Id = id;
    }
  }
}
