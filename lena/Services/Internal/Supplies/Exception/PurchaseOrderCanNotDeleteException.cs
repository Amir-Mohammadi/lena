using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseOrderCanNotDeleteException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }

    public PurchaseOrderCanNotDeleteException(string code)
    {
      this.Code = code;
    }

    public PurchaseOrderCanNotDeleteException(int id)
    {
      this.Id = id;
    }
  }
}
