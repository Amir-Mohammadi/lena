using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class PurchaseOrderDiscountNotFoundException : InternalServiceException
  {
    public int Id { get; set; }

    public PurchaseOrderDiscountNotFoundException(int id)
    {
      Id = id;
    }
  }
}
