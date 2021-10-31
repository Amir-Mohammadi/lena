using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class ActiveReceiptLicenceInput
  {
    public int LadingId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
