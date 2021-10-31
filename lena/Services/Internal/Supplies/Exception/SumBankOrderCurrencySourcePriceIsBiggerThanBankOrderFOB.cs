using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class SumBankOrderCurrencySourcePriceIsBiggerThanBankOrderFOB : InternalServiceException
  {
    public int BankOrderId { get; }

    public SumBankOrderCurrencySourcePriceIsBiggerThanBankOrderFOB(int bankOrderId)
    {
      this.BankOrderId = bankOrderId;
    }
  }
}
