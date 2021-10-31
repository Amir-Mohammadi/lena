using lena.Domains.Enums;
namespace lena.Models.Production.ProductionTerminal
{
  public class ProductionTerminalProductionInput
  {
    public int ProductionOrderId { get; set; }
    public int ProductionTerminalId { get; set; }
    public bool IsFailed { get; set; }
    public string Serial { get; set; }
  }
}
