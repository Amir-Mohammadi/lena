using lena.Services.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{

  public class CannotChangeFinanceStatusToFinancialAcceptException : InternalException
  {
    public string Code { get; set; }

    public CannotChangeFinanceStatusToFinancialAcceptException(string code)
    {
      this.Code = code;
    }
  }
}
