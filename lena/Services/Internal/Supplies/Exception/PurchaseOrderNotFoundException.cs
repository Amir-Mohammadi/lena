using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseOrderNotFoundException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }

    public PurchaseOrderNotFoundException(string code)
    {
      this.Code = code;
    }

    public PurchaseOrderNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
