using lena.Domains.Enums;
namespace lena.Models.Planning.OperationSequence
{
  public class OperationSequenceResult
  {
    public int Id { get; set; }
    public string WorkStationName { get; set; }
    public int WorkStationId { get; set; }
    public int Index { get; set; }
    public bool IsOptional { get; set; }
    public int OperationId { get; set; }
    public int WorkPlanStepId { get; set; }
    public float DefaultTime { get; set; }
    public int WorkStationPartId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public string OperationTitle { get; set; }
    public string WorkStationPartName { get; set; }
    public int WorkStationPartCount { get; set; }
  }
}
