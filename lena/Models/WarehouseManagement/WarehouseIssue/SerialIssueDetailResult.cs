using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseIssue
{
  public class SerialIssueDetailResult
  {
    public int WarehouseIssueId { get; set; }
    public int FromWarehouseId { get; set; }
    public string FromWarehouseName { get; set; }
    public int? ToWarehouseId { get; set; }
    public string ToWarehouseName { get; set; }
    public int? ToDepartmentId { get; set; }
    public string ToDepartmentName { get; set; }
    public int? ToEmployeeId { get; set; }
    public string ToEmployeeName { get; set; }
    public WarehouseIssueStatusType Status { get; set; }
    public string Serial { get; set; }
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int? BillOfMaterialVersionId { get; set; }
    public TransactionLevel TransactionLevel { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? ConfirmDateTime { get; set; }
    public int? ConfirmerUserId { get; set; }
    public string ConfirmerFullName { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
  }
}
