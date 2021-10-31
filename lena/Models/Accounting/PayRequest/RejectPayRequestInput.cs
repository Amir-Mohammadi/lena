using lena.Domains.Enums;
namespace lena.Models.Accounting.PayRequest
{
  public class RejectPayRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
