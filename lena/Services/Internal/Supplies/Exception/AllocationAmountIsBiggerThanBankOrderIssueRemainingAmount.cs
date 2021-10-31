using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class AllocationAmountIsBiggerThanBankOrderIssueRemainingAmount : InternalServiceException
  {
    public int BankOrderId { get; set; }

    public AllocationAmountIsBiggerThanBankOrderIssueRemainingAmount(int bankOrderId)
    {
      this.BankOrderId = bankOrderId;
    }
  }
}
