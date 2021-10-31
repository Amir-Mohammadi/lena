using lena.Domains.Enums;
namespace lena.Models
{
  public class AddDepartmentInput
  {
    public string Name { get; set; }
    public short? ParentDepartmentId { get; set; }
  }
}