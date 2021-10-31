using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Department
{
  public class DepartmentTreeResult
  {
    public DepartmentTreeResult()
    {
      this.ChildDepartments = new List<DepartmentTreeResult>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryLevel { get; set; }
    public byte[] RowVersion { get; set; }

    public IList<DepartmentTreeResult> ChildDepartments { get; set; }
  }
}
