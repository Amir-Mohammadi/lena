using lena.Domains.Enums;
namespace lena.Models
{
  public class ExchangeResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public byte[] RowVersion { get; set; }
  }
}