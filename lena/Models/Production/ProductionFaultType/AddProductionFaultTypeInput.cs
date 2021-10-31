using lena.Domains.Enums;
namespace lena.Models.Production.ProductionFaultType
{
  public class AddProductionFaultTypeInput
  {
    public string Title { get; set; }
    public short? OperationId { get; set; }
    public int[] StuffIds { get; set; }
  }
}
