using lena.Domains.Enums;
namespace lena.Models.Planning.Machine
{
  public class MachineResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int MachineTypeId { get; set; }
    public string MachineTypeName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
