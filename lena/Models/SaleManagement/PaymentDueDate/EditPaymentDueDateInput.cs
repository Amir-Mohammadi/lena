using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PaymentDueDate
{
  public class EditPaymentDueDateInput
  {
    public int Id { get; set; }
    public double Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int PaymentTypeId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
