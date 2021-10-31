using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class ExitReceiptNotFoundException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }

    public ExitReceiptNotFoundException(string code)
    {
      this.Code = code;
    }

    public ExitReceiptNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
