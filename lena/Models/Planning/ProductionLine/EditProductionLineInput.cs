using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLine
{
  public class EditProductionLineInput

  {
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
    public int SortIndex { get; set; }
    public string Name { get; set; }
    public string DetailedCode { get; set; }
    public int ProductivityImpactFactor { get; set; }

    public short ProductWarehouseId { get; set; }
    public short ConsumeWarehouseId { get; set; }
    public short DepartmentId { get; set; }
    public string Description { get; set; }
    public int? RepairUnitId { get; set; }
    public int[] AddedProductionSteps { get; set; }
    public int[] DeletedProductionSteps { get; set; }
  }
}
