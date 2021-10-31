using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseReports
{

  public class WarehouseEnterExitResult
  {

    public int WarehouseIssueId { get; set; }
    public int StuffId { get; set; }
    public int StuffCategoryId { get; set; }
    public string StuffCategoryName { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNone { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public int FromWarehouseId { get; set; }
    public string FromWarehouseName { get; set; }
    public int? ToWarehouseId { get; set; }
    public string ToWarehouseName { get; set; }
    public int? ToDepartmentId { get; set; }
    public string ToDepartmentName { get; set; }
    public int? ToEmployeeId { get; set; }
    public string ToEmployeeName { get; set; }
    public WarehouseIssueStatusType Status { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public int WarehouseIssueItemId { get; set; }
    public TransactionLevel TransactionLevel { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public DateTime DateTime { get; set; }
  }


  //public class WarehouseEnterExitResult
  //{
  //    public int? WarehouseId { get; set; }
  //    public string WarehouseName { get; set; }
  //    public int? DepartmentId { get; set; }
  //    public string DepartmentName { get; set; }
  //    public int? ToEmployeeId { get; set; }
  //    public string toEmployeeName { get; set; }
  //    public double? TotalEnterQty { get; set; }
  //    public double? TotalExitQty { get; set; }
  //    public IQueryable<WarehouseEnterExitStuff> Stuffs { get; set; }

  //}


}
