using lena.Domains.Enums;
namespace lena.Models.Planning.WorkPlanStep
{
  public class WorkPlanStepResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public long InitialTime { get; set; }
    public long SwitchTime { get; set; }
    public long BatchTime { get; set; }
    public double BatchCount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double ConversionRatio { get; set; }
    public int WorkPlanId { get; set; }
    public int WorkPlanBillOfMaterialStuffId { get; set; }
    public int WorkPlanBillOfMaterialVersion { get; set; }
    public string WorkPlanTitle { get; set; }
    public int ProductionStepId { get; set; }
    public string ProductionStepName { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool WorkPlanIsPublished { get; set; }
    public bool NeedToQualityControl { get; set; }
    public bool PlanningWithoutMachineLimit { get; set; }
    public float? SumOfOperationSequenceTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
