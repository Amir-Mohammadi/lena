using System.Linq;
using lena.Models.Planning.OperationConsumingMaterial;

using lena.Domains.Enums;
namespace lena.Models.Planning.OperationSequence
{
  public class FullOperationSequenceResult
  {
    public int Id { get; set; }
    public int WorkStationId { get; set; }
    public string WorkStationName { get; set; }
    public bool IsOptional { get; set; }
    public bool IsRepairReturnPoint { get; set; }
    public int Index { get; set; }
    public int WorkStationOperationId { get; set; }
    public float DefaultTime { get; set; }
    public int WorkPlanStepId { get; set; }
    public int WorkStationPartId { get; set; }
    public int WorkStationPartCount { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public IQueryable<FullOperationConsumingMaterialResult> OperationConsumingMaterials { get; set; }
  }
}
