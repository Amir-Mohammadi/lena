using lena.Domains.Enums;
namespace lena.Models.Production.ProductionFaultType
{
  public class EditProductionFaultTypeInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public short? OperationId { get; set; }
    public int[] AddStuffIds { get; set; }
    public int[] DeleteStuffIds { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
