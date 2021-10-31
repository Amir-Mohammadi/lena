using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequestEditLog
{
  public class PurchaseRequestEditLogResult
  {
    public int Id { get; set; }
    public int PurchaseRequestId { get; set; }
    public DateTime? BeforeDeadLineDateTime { get; set; }
    public DateTime? AfterDeadLineDateTime { get; set; }
    public double? BeforeRequestQty { get; set; }
    public double? AfterRequestQty { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public string EmployeeName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
