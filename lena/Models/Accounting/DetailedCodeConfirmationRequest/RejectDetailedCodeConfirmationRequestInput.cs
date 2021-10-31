using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.DetailedCodeConfirmationRequest
{
  public class RejectDetailedCodeConfirmationRequestInput
  {
    public int Id { get; set; }
    public string DetailedCode { get; set; }
    public DetailedCodeEntityType DetailedCodeEntityType { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
