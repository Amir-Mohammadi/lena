using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionStep
{
  public class AddProductionStepInput
  {
    public string Name { get; set; }

    public string Description { get; set; }

    public int ProductivityImpactFactor { get; set; }

    public int[] AddedWorkstations { get; set; }
  }
}
