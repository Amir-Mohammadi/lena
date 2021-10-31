using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class BankOrderHasEnactmentException : InternalServiceException
  {
    public int BankOrderId { get; set; }

    public BankOrderHasEnactmentException(int bankOrderId)
    {
      this.BankOrderId = bankOrderId;
    }
  }
}