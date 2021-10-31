using lena.Models.Planning.OperationConsumingMaterial;
using lena.Models.Planning.OperationSequenceMachineTypeParameter;

using lena.Domains.Enums;
namespace lena.Models.Planning.OperationSequence
{
  public class EditOperationSequenceInput
  {
    public int Id { get; set; }
    public short WorkStationId { get; set; }
    public int Index { get; set; }
    public bool IsOptional { get; set; }
    public bool IsRepairReturnPoint { get; set; }
    public short WorkStationOperationId { get; set; }
    public float DefaultTime { get; set; }
    public short WorkStationPartId { get; set; }
    public string Description { get; set; }
    public int WorkStationPartCount { get; set; }
    public AddOperationConsumingMaterialInput[] AddOperationConsumingMaterialInputs { get; set; }
    public EditOperationConsumingMaterialInput[] EditOperationConsumingMaterialInputs { get; set; }
    public int[] DeleteOperationConsumingMaterialInputs { get; set; }
    public AddOperationSequenceMachineTypeParameterInput[] AddOperationSequenceMachineTypeParameterInputs { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
