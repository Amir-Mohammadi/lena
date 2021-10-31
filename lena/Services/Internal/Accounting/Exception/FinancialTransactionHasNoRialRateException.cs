using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialTransactionHasNoRialRateException : InternalServiceException
  {
    public int FinancialTransactionId { get; set; }
    public string FinancialAccountCode { get; set; }

    public FinancialTransactionHasNoRialRateException(int financialTransactionId, string financialAccountCode)
    {
      FinancialTransactionId = financialTransactionId;
      FinancialAccountCode = financialAccountCode;
    }
  }
}
