using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemChangeConfirmations
{
  public class OrderItemChangeConfirmationResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public bool Confirmed { get; set; }
    public int OrderItemChangeRequestId { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
