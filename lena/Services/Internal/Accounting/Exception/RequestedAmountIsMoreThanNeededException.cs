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
  public class RequestedAmountIsMoreThanNeededException : InternalException
  {
    public double? AllocatedAmount { get; set; }
    public double RequestedAmount { get; set; }
    public double? TotalAmount { get; set; }
    public int? PurchaseOrderId { get; set; }

    public int? ExpenseFinancialDocumentId { get; set; }

    public RequestedAmountIsMoreThanNeededException(int? purchaseOrderId, double? totalAmount, double? allocatedAmount, double requestedAmount, int? expenseFinancialDocumentId)
    {
      this.AllocatedAmount = allocatedAmount;
      this.RequestedAmount = requestedAmount;
      this.TotalAmount = TotalAmount;
      this.PurchaseOrderId = purchaseOrderId;
      this.ExpenseFinancialDocumentId = expenseFinancialDocumentId;
    }

  }
}
