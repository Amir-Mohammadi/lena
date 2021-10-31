using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrder
{
  public class ProductionOrderBillOfMaterialDetailResult
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int? Version { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double MainUnitQty { get; set; }
    public int MainUnitId { get; set; }
    public string MainUnitName { get; set; }
    public int MainUnitDecimalDigitCount { get; set; }
    public int DefaultWarehouseId { get; set; }
    public string DefaultWarehouseName { get; set; }
    public int ConsumeWarehouseId { get; set; }
    public string ConsumeWarehouseName { get; set; }
    public double ConsumeWarehouseAmount { get; set; }
    public double DefaultWarehouseAmount { get; set; }
    public int? StockUnitId { get; set; }
    public byte DecimalDigitCount { get; set; }
    public string StockUnitName { get; set; }
    public double FaultyPercentage { get; set; }
    public double BillOfMaterialDetailValue { get; set; }
  }
}
