using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.ApiInfo
{
  public class DeleteApiInfoInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
