using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class SumBankOrderCurrencySourceTransferCostIsBiggerThanBankOrderTransferCost : InternalServiceException
  {
    public int BankOrderId { get; }

    public SumBankOrderCurrencySourceTransferCostIsBiggerThanBankOrderTransferCost(int bankOrderId)
    {
      this.BankOrderId = bankOrderId;
    }
  }
}
