using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class EditQualityControlTestOperationInput
  {
    public int Id { get; set; }
    public int TestOperationId { get; set; }
    public long QualityControlTestId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
