using lena.Domains.Enums;
namespace lena.Models.Planning.MachineTypeOperatorType
{
  public class EditMachineTypeOperatorTypeInput
  {
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public short MachineTypeId { get; set; }
    public short OperatorTypeId { get; set; }
    public bool IsNecessary { get; set; }
  }
}
