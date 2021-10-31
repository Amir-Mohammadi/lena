using System;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.BaseEntityDocument
{
  public class EditBaseEntityDocumentInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public Guid? DocumentId { get; set; }
    public int? BaseEntityDocumentTypeId { get; set; }
    public int? CooperatorId { get; set; }
    public string Description { get; set; }
  }
}
