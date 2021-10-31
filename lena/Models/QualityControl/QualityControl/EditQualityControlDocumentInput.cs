using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControl
{
  public class EditQualityControlDocumentInput
  {
    public int Id { get; set; }
    public string FileKey { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
