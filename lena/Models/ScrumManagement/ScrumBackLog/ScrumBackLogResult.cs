using lena.Models.ScrumManagement.ScrumTask;

using lena.Domains.Enums;
namespace lena.Models.ScrumManagement.ScrumBackLog
{
  public class ScrumBackLogResult
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
    public int ScrumSprintId { get; set; }
    public string ScrumSprintName { get; set; }
    public System.DateTime? DateTime { get; set; }
    public ScrumTaskResult[] Tasks { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
