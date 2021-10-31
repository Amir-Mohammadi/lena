using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.BaseEntityDocument
{
  public class AddBaseEntityDocumentInput
  {
    public string Description { get; set; }
    public int BaseEntityId { get; set; }
    public int? BaseEntityDocumentTypeId { get; set; }
    public int? CooperatorId { get; set; }
    public string FileKey { get; set; }
    public int[] BaseEntityDocumentIds { get; set; }
  }
}
