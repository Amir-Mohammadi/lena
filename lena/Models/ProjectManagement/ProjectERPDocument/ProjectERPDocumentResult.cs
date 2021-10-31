using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPDocument
{
  public class ProjectERPDocumentResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime CreateDateTime { get; set; }
    public Guid DocumentId { get; set; }
    public string Description { get; set; }
    public short ProjectERPDocumentTypeId { get; set; }
    public string ProjectERPDocumentTypeName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}