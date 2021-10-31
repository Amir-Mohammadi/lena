using lena.Domains.Enums;
namespace lena.Models.Planning.MachineTypeParameter
{
  public class MachineTypeParameterResult
  {
    public int Id { get; set; }
    public int MachineTypeId { get; set; }
    public string MachineTypeName { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
