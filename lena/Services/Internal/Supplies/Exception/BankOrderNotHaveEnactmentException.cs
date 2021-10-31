using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class BankOrderNotHaveEnactmentException : InternalServiceException
  {

    public int BankOrderId { get; set; }
    public BankOrderNotHaveEnactmentException(int bankOrderId)
    {
      this.BankOrderId = bankOrderId;
    }

  }
}
