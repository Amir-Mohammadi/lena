using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseIssueItem
{
  public class WarehouseIssueItemResult
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
  }
}
