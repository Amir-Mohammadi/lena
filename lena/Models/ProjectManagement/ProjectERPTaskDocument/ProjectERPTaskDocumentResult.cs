using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPTaskDocument
{
  public class ProjectERPTaskDocumentResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime CreateDateTime { get; set; }
    public Guid DocumentId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}