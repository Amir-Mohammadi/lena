using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class ExitReceiptRequestSummaryForExitReceiptRequestNotFoundException : InternalServiceException
  {
    public int ExitReceiptRequestId { get; }

    public ExitReceiptRequestSummaryForExitReceiptRequestNotFoundException(int exitReceiptRequestId)
    {
      this.ExitReceiptRequestId = exitReceiptRequestId;
    }
  }
}
