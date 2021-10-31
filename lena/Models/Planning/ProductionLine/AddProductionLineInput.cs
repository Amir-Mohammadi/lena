using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLine
{
  public class AddProductionLineInput
  {
    public int? AdminUserGroupId { get; set; }

    public string Name { get; set; }
    public string DetailedCode { get; set; } //کد تفصیلی
    public int ProductivityImpactFactor { get; set; }
    public int SortIndex { get; set; }
    public string Description { get; set; }
    public short DepartmentId { get; set; }
    public short ConsumeWarehouseId { get; set; }
    public short ProductWarehouseId { get; set; }
    public int[] ProductionSteps { get; set; }
    public int? RepairUnitId { get; set; }

  }
}
