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
  public class CanNotDeleteUsedRialRateException : InternalServiceException
  {
    public int FinancialTransactionId { get; set; }
    public string FinancialAccountCode { get; set; }

    public CanNotDeleteUsedRialRateException(int financialTransactionId, string financialAccountCode)
    {
      FinancialTransactionId = financialTransactionId;
      FinancialAccountCode = financialAccountCode;
    }
  }
}
