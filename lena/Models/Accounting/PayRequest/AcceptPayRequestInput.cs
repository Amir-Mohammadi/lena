using lena.Domains.Enums;
namespace lena.Models.Accounting.PayRequest
{
  public class AcceptPayRequestInput
  {
    public int Id { get; set; }
    public string FileKey { get; set; }
    public double TransactionAmount { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
