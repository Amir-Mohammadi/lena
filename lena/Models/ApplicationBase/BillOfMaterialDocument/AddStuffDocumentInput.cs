using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.StuffDocument
{
  public class AddStuffDocumentInput
  {
    public int StuffId { get; set; }
    public string Title { get; set; }
    public string FileKey { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public StuffDocumentType StuffDocumentType { get; set; }
  }
}
