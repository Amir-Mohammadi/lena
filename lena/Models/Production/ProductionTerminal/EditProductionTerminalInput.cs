using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionTerminal
{
  public class EditProductionTerminalInput
  {
    public int Id { get; set; }

    public ProductionTerminalType ProductionTerminalType { get; set; }
    public int ProductionLineId { get; set; }
    public int NetworkId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
