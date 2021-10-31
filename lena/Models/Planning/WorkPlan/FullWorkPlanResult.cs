using System;
using lena.Models.Planning.BillOfMaterialDetail;
using lena.Models.Planning.ProductionLineProductionStep;
using lena.Models.Planning.WorkPlanStep;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkPlan
{
  public class FullWorkPlanResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int Version { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public int BillOfMaterialVersion { get; set; }
    public int QtyPerBox { get; set; }
    public bool IsActive { get; set; }

    public bool IsPublished { get; set; }
    public DateTime CreateDate { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string Description { get; set; }
    public FullWorkPlanStepResult[] WorkPlanSteps { get; set; }
    public byte[] RowVersion { get; set; }
    public int ProductionStepId { get; set; }
    public string ProductionStepName { get; set; }
    public BillOfMaterialDetailResult[] BillOfMaterialDetails { get; set; }
    public ProductionLineProductionStepResult[] ProductionLineProductionSteps { get; set; }
  }
}
