using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.StuffDocument
{
  public class StuffDocumentResult
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public string DocumentTitle { get; set; }
    public string DocumentDescription { get; set; }
    public StuffDocumentType StuffDocumentType { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; }
    public DateTime DateTime { get; set; }
    public string FileName { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}