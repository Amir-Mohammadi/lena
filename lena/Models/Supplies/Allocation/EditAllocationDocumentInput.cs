using lena.Domains.Enums;
namespace lena.Models
{
  public class EditAllocationDocumentInput
  {
    public int Id { get; set; }
    public string FileKey { get; set; }
    public byte[] RowVersion { get; set; }
  }
}