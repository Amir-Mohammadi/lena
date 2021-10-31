using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrder
{
  public class EditProductionOrderWorkPlanStepInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int WorkPlanStepId { get; set; }
  }
}
