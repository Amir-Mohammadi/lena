using lena.Domains.Enums;
namespace lena.Models.Production.ProductionTerminal
{
  public class AddProductionTerminalInput
  {
    public int ProductionLineId { get; set; }
    public int? TerminalOrder { get; set; }
    public int NetworkId { get; set; }
  }
}
