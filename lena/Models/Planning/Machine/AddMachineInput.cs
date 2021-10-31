using lena.Domains.Enums;
namespace lena.Models.Planning.Machine
{
  public class AddMachineInput
  {
    public string Name { get; set; }
    public short MachineTypeId { get; set; }
    public string Description { get; set; }
  }
}
