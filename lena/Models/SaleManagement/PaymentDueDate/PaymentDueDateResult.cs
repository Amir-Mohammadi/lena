using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PaymentDueDate
{
  public class PaymentDueDateResult
  {
    public int Id { get; set; }
    public int? OrderId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int PaymentTypeId { get; set; }
    public string PaymentTypeName { get; set; }
    public DateTime? PaymentDate { get; set; }
    public Double? Amount { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
