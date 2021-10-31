using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.BaseEntityDocumentType
{
  public class BaseEntityDocumentTypeResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EntityType? EntityType { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
