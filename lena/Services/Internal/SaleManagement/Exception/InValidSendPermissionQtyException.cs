using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class InValidSendPermissionQtyException : InternalServiceException
  {
    public int ExitReceiptRequestId { get; }
    public double TotalQty { get; set; }

    public InValidSendPermissionQtyException(int exitReceiptRequestId, double totalQty)
    {
      this.ExitReceiptRequestId = exitReceiptRequestId;
      TotalQty = totalQty;
    }
  }
}
