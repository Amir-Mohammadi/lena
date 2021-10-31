using lena.Domains.Enums;
namespace lena.Models
{
  public class EditDepartmentInput
  {
    public short Id { get; set; }
    public string Name { get; set; }
    public short? ParentDepartmentId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
