using lena.Domains.Enums;
namespace lena.Models.Production.Production
{
  public class GetProductionConsumingMaterialsInput
  {
    public int ProductionOrderId { get; set; }
    public int? ProductionTerminalId { get; set; }
    public int[] ProductionOperationSequenceIds { get; set; }
  }
}
