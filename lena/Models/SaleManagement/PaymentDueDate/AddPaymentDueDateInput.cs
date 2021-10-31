using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PaymentDueDate
{
  public class AddPaymentDueDateInput
  {
    public double Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int PaymentTypeId { get; set; }

  }
}
