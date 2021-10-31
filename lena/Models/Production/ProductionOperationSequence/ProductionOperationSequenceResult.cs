using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperationSequence
{
  public class ProductionOperationSequenceResult
  {
    public int Id { get; set; }
    public int WorkPlanStepId { get; set; }
    public int OperationId { get; set; }
    public string OperationTitle { get; set; }
    public int Index { get; set; }
    public bool IsOptional { get; set; }
    public int? MachineTypeId { get; set; }
    public string MachineTypeName { get; set; }
    public int WorkStationPartId { get; set; }
    public int WorkStationPartCount { get; set; }
    public int? OperatorTypeId { get; set; }
    public string OperatorTypeName { get; set; }
    public float DefaultTime { get; set; }
    public int? MachineTypeOperatorTypeId { get; set; }
    public string MachineTypeOperatorTypeTitle { get; set; }
    public int OperationCount { get; set; }
    public string WorkStationName { get; set; }
    public int WorkStationId { get; set; }
    public double BatchCount { get; set; }
    public long SwitchTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
