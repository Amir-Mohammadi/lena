using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.Machine
{
  public class EditMachineInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public TValue<string> Name { get; set; }
    public TValue<short> MachineTypeId { get; set; }
    public TValue<string> Description { get; set; }
  }
}
