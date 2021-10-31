using lena.Domains.Enums;
namespace lena.Models
{
  public class EditCustomerStuffVersionInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public int CustomerStuffId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}