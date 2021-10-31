using lena.Models.QualityControl.SerialFailedOperationFaultOperationEmployee;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.SerialFailedOperationFualtOperation
{
  public class SaveSerialFailedOperationFaultOperationInput
  {
    public int SerialFailedOperationId { get; set; }
    public string SerialFailedOperationDescription { get; set; }
    public AddSerialFailedOperationFaultOperationInput[] AddSerialFailedOperationFaultOperationInputs { get; set; }
    public int[] DeletedSerailFailedOperationFaultIds { get; set; }
    public DeleteSerialFailedOperationFaultOperationEmployeeInput[] DeleteSerialFailedOperationFaultOperationEmployeeInputs { get; set; }
    public AddSerialFailedOperationFaultOperationEmployeeInput[] AddSerialFailedOperationFaultOperationEmployeeInputs { get; set; }
  }
}
