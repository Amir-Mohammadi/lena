using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PaymentDueDate
{
  public class PaymentDueDateGroupKey
  {
    public int? CustomerId { get; set; }
    public int? PaymentTypeId { get; set; }
    public DateTime? PaymentDate { get; set; }
  }
}
