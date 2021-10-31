using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class BankOrderCurrencySourceTransferCostCanNotBiggerThanBankOrderTransferCostException : InternalServiceException
  {
    public int BankOrderCurrencySourceId { get; set; }
    public int BankOrderId { get; set; }
    public BankOrderCurrencySourceTransferCostCanNotBiggerThanBankOrderTransferCostException(int bankOrderCurrencySourceId, int bankOrderId)
    {
      this.BankOrderCurrencySourceId = bankOrderCurrencySourceId;
      this.BankOrderId = bankOrderId;

    }

  }
}
