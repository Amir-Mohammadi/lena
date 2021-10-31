using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseIssueItem
{
  public class WarehouseIssueItemFullResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public int WarehouseIssueId { get; set; }
    public string EmployeeFullName { get; set; }
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string Serial { get; set; }
    public long? SerialCode { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public string QualityControlDescription { get; set; }
    public bool IsDelete { get; set; }
    public string AssetCode { get; set; }
    public byte[] RowVersion { get; set; }
    public string WarehouseIssueCode { get; set; }
    public DateTime WarehouseIssueDateTime { get; set; }
    public int WarehouseIssueFromWarehouseId { get; set; }
    public string WarehouseIssueFromWarehouseName { get; set; }
    public int? WarehouseIssueToWarehouseId { get; set; }
    public string WarehouseIssueToWarehouseName { get; set; }
    public WarehouseIssueStatusType WarehouseIssueStatus { get; set; }
    public string WarehouseIssueUserName { get; set; }
    public int? WarehouseIssueEmployeeId { get; set; }
    public string WarehouseIssueEmployeeFullName { get; set; }
    public string WarehouseIssueResponseUserName { get; set; }
    public DateTime? WarehouseIssueResponseDateTime { get; set; }
    public string WarehouseIssueResponseEmployeeFullName { get; set; }
    public int? WarehouseIssueToDepartmentId { get; set; }
    public string WarehouseIssueToDepartmentName { get; set; }
    public int? WarehouseIssueToEmployeeId { get; set; }
    public string WarehouseIssueToEmployeeFullName { get; set; }
    public string WarehouseIssueConfirmDescription { get; set; }
    public byte[] WarehouseIssueRowVersion { get; set; }
  }
}
