using lena.Domains.Enums;
namespace lena.Models
{
  public class UnPublishCustomerStuffVersionInput
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public byte[] RowVersion { get; set; }
  }
}