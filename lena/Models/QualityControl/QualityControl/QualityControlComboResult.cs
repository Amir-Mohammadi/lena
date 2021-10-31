using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControl
{
  public class QualityControlComboResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public QualityControlType QualityControlType { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
