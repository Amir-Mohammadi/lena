using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLine
{
  public class ProductionLineResult
  {
    public int Id { get; set; }
    public int SortIndex { get; set; }
    public string Name { get; set; }
    public string DetailedCode { get; set; }
    public bool ConfirmationDetailedCode { get; set; }
    public int ProductivityImpactFactor { get; set; }
    public string Description { get; set; }
    public int DepartmentId { get; set; }
    public int ConsumeWarehouseId { get; set; }
    public int ProductWarehouseId { get; set; }
    public string DepartmentName { get; set; }
    public string ConsumeWarehouseName { get; set; }
    public string ProductWarehouseName { get; set; }
    public int[] ProductionSteps { get; set; }
    public string Barcode { get; set; }
    public int? RepairUnitId { get; set; }
    public string RepairUnitName { get; set; }
    public int? RepairUnitWarehouseId { get; set; }
    public string RepairUnitWarehouseName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
