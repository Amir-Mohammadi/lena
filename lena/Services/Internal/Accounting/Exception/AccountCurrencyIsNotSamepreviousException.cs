using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class AccountCurrencyIsNotSamePreviousException : InternalServiceException
  {
    public string FinancialAccountCode { get; set; }
    public string FinancialAccountCurrencyTitle { get; set; }
    public string PreviousFinancialAccountCode { get; set; }
    public string PreviousFinancialAccountCurrencyTitle { get; set; }

    public AccountCurrencyIsNotSamePreviousException(
                                string financialAccountCode,
                                string financialAccountCurrencyTitle,
                                string previousFinancialAccountCode,
                                string previousFinancialAccountCurrencyTitle)
    {
      FinancialAccountCode = financialAccountCode;
      FinancialAccountCurrencyTitle = financialAccountCurrencyTitle;
      PreviousFinancialAccountCode = previousFinancialAccountCode;
      PreviousFinancialAccountCurrencyTitle = previousFinancialAccountCurrencyTitle;
    }
  }
}
