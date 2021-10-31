using lena.Domains.Enums;
namespace lena.Models.Planning.MachineTypeParameter
{
  public class EditMachineTypeParameterInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public short MachineTypeId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
