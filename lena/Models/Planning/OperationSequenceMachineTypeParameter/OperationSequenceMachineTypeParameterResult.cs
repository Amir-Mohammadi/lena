using lena.Domains.Enums;
namespace lena.Models.Planning.OperationSequenceMachineTypeParameter
{
  public class OperationSequenceMachineTypeParameterResult
  {
    public int Id { get; set; }
    public int OperationSequenceId { get; set; }
    public int MachineTypeParameterId { get; set; }
    public string MachineTypeParameterName { get; set; }
    public double Value { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
