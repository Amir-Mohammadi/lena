using lena.Models.QualityControl.SerialFailedOperationFaultOperationEmployee;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.SerialFailedOperationFualtOperation
{
  public class AddSerialFailedOperationFaultOperationInput
  {
    public int? Id { get; set; }
    public short OperationId { get; set; }
    public AddSerialFailedOperationFaultOperationEmployeeInput[] AddSerialFailedOperationFaultOperationEmployeeInputs { get; set; }
  }
}
