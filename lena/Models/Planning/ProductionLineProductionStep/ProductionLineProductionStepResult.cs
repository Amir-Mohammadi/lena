using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLineProductionStep
{
  public class ProductionLineProductionStepResult
  {
    public int ProductionStepId { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionStepName { get; set; }
    public string ProductionLineName { get; set; }
    public byte[] RowVersion { get; set; }
    public string DepartmentName { get; set; }
    public int DepartmentId { get; set; }
  }
}
