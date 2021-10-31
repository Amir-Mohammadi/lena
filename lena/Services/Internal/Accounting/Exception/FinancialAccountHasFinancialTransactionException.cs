using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialAccountHasFinancialTransactionException : InternalServiceException
  {
    public string Code { get; }

    public FinancialAccountHasFinancialTransactionException(string code)
    {
      this.Code = code;
    }
  }
}
