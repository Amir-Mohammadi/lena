using System;

using lena.Domains.Enums;
namespace lena.Models.Production.RepairUnit
{
  public class ProductionLineRepairUnitResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public DateTime CreationTime { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }

  public class FullProductionLineRepairUnitResult : ProductionLineRepairUnitResult
  {
    public int? ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
  }
}
