using lena.Domains.Enums;
namespace lena.Models
{
  public class CooperatorResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public string Code { get; set; }
  }
}
