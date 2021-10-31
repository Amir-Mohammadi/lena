using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectHeader
{
  public class ProjectHeaderResult
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
    public int OwnerCustomerId { get; set; }
    public string OwnerCustomerCode { get; set; }
    public string OwnerCustomerName { get; set; }
    public string StuffName { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public byte[] RowVersion { get; set; }
  }
}