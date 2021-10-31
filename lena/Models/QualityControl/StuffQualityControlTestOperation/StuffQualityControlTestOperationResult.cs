using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class StuffQualityControlTestOperationResult
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public int QualityControlOperationTestOperationId { get; set; }
    public long QualityControlTestOperationQualityControlTestId { get; set; }

    public byte[] RowVersion { get; set; }
  }
}
