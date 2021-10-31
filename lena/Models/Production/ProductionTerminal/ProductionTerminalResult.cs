using lena.Domains.Enums;
namespace lena.Models.Production.ProductionTerminal
{
  public class ProductionTerminalResult
  {
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public int NetworkId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
