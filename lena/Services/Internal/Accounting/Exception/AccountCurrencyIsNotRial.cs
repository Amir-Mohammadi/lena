using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class AccountCurrencyIsNotRial : InternalServiceException
  {
    public string FinancialAccountCode { get; set; }

    public AccountCurrencyIsNotRial(string financialAccountCode)
    {
      FinancialAccountCode = financialAccountCode;
    }
  }
}
