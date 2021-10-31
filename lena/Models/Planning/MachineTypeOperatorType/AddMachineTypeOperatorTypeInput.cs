using lena.Domains.Enums;
namespace lena.Models.Planning.MachineTypeOperatorType
{
  public class AddMachineTypeOperatorTypeInput
  {
    public string Title { get; set; }
    public short MachineTypeId { get; set; }
    public short OperatorTypeId { get; set; }
    public bool IsNecessary { get; set; }
  }
}
