using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionStep
{
  public class EditProductionStepInput : AddProductionStepInput
  {
    public int Id { get; set; }
    public int[] DeletedWorkstations { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
