using lena.Domains.Enums;
namespace lena.Models.ScrumManagement.ScrumProject
{
  public class ScrumProjectResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public bool IsCommit { get; set; }
    public bool IsDelete { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public long EstimatedTime { get; set; }
    public byte[] RowVersion { get; set; }
    public int ScrumProjectGroupId { get; set; }
    public string ScrumProjectGroupName { get; set; }
  }
}
