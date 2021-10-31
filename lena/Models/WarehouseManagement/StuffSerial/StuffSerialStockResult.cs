using System.Collections.Generic;
using lena.Domains;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  public class StuffSerialStockResult
  {
    public StuffSerialStockResult()
    {
      WarehouseInventories = new List<WarehouseInventoryResult>();
    }
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public int? Version { get; set; }
    public string Serial { get; set; }
    public long StuffSerialCode { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double UnitConversionRatio { get; set; }
    public int? WarehouseId { get; set; }
    public List<WarehouseInventoryResult> WarehouseInventories { get; set; }
  }
}
