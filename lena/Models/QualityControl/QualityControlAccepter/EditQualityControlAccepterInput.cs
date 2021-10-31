using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlAccepter
{
  public class EditQualityControlAccepterInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int UserGroupId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
