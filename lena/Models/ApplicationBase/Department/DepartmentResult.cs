using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Department
{
  public class DepartmentResult
  {
    public short Id { get; set; }
    public string Name { get; set; }
    public short? ParentDepartmentId { get; set; }
    public string ParentDepartmentName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
