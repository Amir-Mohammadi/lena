using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStation
{
  public class AddWorkStationInput
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProductionLineId { get; set; }
    public int ConsumeWarehouseId { get; set; }
    public int ProductWarehouseId { get; set; }
    public int[] ProductionSteps { get; set; }
  }
}
