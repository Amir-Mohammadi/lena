using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseOrderDetailNotFoundException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }

    public PurchaseOrderDetailNotFoundException(string code)
    {
      this.Code = code;
    }

    public PurchaseOrderDetailNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
