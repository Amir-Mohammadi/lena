using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.BaseEntityDocumentType
{
  public class AddBaseEntityDocumentTypeInput
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public EntityType? EntityType { get; set; }
  }
}
