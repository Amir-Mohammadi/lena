using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlTestUnit
{
  public class EditQualityControlTestUnitInput : AddQualityControlTestUnitInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
