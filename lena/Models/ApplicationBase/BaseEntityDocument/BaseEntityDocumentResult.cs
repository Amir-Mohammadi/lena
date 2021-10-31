using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.BaseEntityDocument
{
  public class BaseEntityDocumentResult
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsDelete { get; set; }
    public DateTime DateTime { get; set; }
    public int? BaseEntityId { get; set; }
    public int UserId { get; set; }
    public Guid? DocumentId { get; set; }
    public int? BaseEntityDocumentTypeId { get; set; }
    public int? CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public string BaseEntityDocumentTypeTitle { get; set; }
    public string BaseEntityEntityDescription { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
    public int EmployeeId { get; set; }
    public EntityType? EntityType { get; set; }
    public bool Appendix { get; set; }
  }
}
