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
  public class ThereIsAlreadyOneFinancialDocumentCorrectionWithTheSameDateTimeException : InternalServiceException
  {
    public int FinancialDocumentId { get; set; }
    public ThereIsAlreadyOneFinancialDocumentCorrectionWithTheSameDateTimeException(int financialDocumentId)
    {
      FinancialDocumentId = financialDocumentId;
    }
  }
}
