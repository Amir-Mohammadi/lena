using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class TestOperationResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
