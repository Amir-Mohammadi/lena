using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseIssue
{
  public class WarehouseIssueResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime DateTime { get; set; }
    public int FromWarehouseId { get; set; }
    public string FromWarehouseName { get; set; }
    public int? ToWarehouseId { get; set; }
    public string ToWarehouseName { get; set; }
    public WarehouseIssueStatusType Status { get; set; }
    public string UserName { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public string ResponseUserName { get; set; }
    public DateTime? ResponseDateTime { get; set; }
    public string ResponseEmployeeFullName { get; set; }
    public int? ToDepartmentId { get; set; }
    public string ToDepartmentName { get; set; }
    public int? ToEmployeeId { get; set; }
    public string ToEmployeeFullName { get; set; }
    public string ConfirmDescription { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
