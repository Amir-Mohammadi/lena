using lena.Models.Production.ProductionOperation;

using lena.Domains.Enums;
namespace lena.Models.Production.Production
{
  public class AddProductionInput
  {
    public int ProductionOrderId { get; set; }
    public int ProductionTerminalId { get; set; }
    public bool IsFailed { get; set; }
    public string Serial { get; set; }
    public string Description { get; set; }
    public AddProductionOperationInput[] AddProductionOperations { get; set; }
  }
}
