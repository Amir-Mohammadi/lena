using lena.Models.Planning.OperationSequence;
using lena.Models.Planning.WorkStationPart;
using lena.Models.Planning.WorkStation;
using lena.Models.Planning.WorkStationOperation;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkPlanStep
{
  public class FullWorkPlanStepResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public long InitialTime { get; set; }
    public long SwitchTime { get; set; }
    public long BatchTime { get; set; }
    public double BatchCount { get; set; }
    public int WorkPlanId { get; set; }
    public int ProductionStepId { get; set; }
    public int ProductionLineId { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool NeedToQualityControl { get; set; }
    public bool PlanningWithoutMachineLimit { get; set; }
    public FullOperationSequenceResult[] OperationSequences { get; set; }
    public WorkStationResult[] WorkStations { get; set; }
    public WorkStationPartComboResult[] WorkStationParts { get; set; }
    public WorkStationOperationResult[] WorkStationOperations { get; set; }
    public byte[] RowVersion { get; set; }
    public int? ProductWarehouseId { get; set; }
    public int? ConsumeWarehouseId { get; set; }
    public string ProductinoStepName { get; set; }
    public string ProductionLineName { get; set; }
  }
}
