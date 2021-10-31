using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AssetLogResult
  {
    public int? AssetId { get; set; }
    public string AssetCode { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public short? DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public long? Serial { get; set; }
    public int? UserId { get; set; }
    public string UserFullName { get; set; }
    public int? WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public DateTime CreateDateTime { get; set; }
    public string Description { get; set; }
  }
}
