using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPEventDocument
{
  public class ProjectERPEventDocumentResult
  {
    public int Id { get; set; }
    public int CreatorUserId { get; set; }
    public string CreatorEmployeeFullName { get; set; }
    public DateTime CreationDateTime { get; set; }
    public Guid DocumentId { get; set; }
    public string Description { get; set; }
    public short ProjectERPEventDocumentTypeId { get; set; }
    public string ProjectERPEventDocumentTypeName { get; set; }
    public byte[] RowVersion { get; set; }

    public string FileName { get; set; }
    public double FileSize { get; set; }
    public string FileType { get; set; }
  }
}