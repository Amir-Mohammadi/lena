using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlTestUnit
{
  public class QualityControlTestUnitResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
