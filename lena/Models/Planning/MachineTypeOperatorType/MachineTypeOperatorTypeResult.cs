using lena.Domains.Enums;
namespace lena.Models.Planning.MachineTypeOperatorType
{
  public class MachineTypeOperatorTypeResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int MachineTypeId { get; set; }
    public string MachineTypeName { get; set; }
    public int OperatorTypeId { get; set; }
    public string OperatorTypeName { get; set; }
    public bool IsNecessary { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
