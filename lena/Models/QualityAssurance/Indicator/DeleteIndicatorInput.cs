using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.Indicator
{
  public class DeleteIndicatorInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
