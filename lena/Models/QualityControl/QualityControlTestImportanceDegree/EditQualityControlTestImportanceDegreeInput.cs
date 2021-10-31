using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class EditQualityControlTestImportanceDegreeInput
  {
    public int Id { get; set; }
    public int TestImportanceDegreeId { get; set; }
    public long QualityControlTestId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
