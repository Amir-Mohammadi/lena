using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseReports
{
  public class WarehouseEnterExitSerial
  {
    public string Serial { get; set; }
    public double? Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string EmployeeFullName { get; set; }
  }

}
