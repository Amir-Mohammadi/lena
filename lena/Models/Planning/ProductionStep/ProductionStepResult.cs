using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionStep
{
  public class ProductionStepResult
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public int ProductivityImpactFactor { get; set; }
    public int[] ProductionLines { get; set; }
  }
}
