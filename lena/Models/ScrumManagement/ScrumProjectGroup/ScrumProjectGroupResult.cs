using lena.Domains.Enums;
namespace lena.Models.ScrumManagement.ScrumProjectGroup
{
  public class ScrumProjectGroupResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long EstimatedTime { get; set; }
    public bool IsDelete { get; set; }
    public bool IsCommit { get; set; }
    public byte[] RowVersion { get; set; }
    public string Color { get; set; }
  }
}
