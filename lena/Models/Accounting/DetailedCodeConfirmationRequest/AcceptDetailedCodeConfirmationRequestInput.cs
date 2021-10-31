using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.DetailedCodeConfirmationRequest
{
  public class AcceptDetailedCodeConfirmationRequestInput
  {
    public int Id { get; set; }
    public DetailedCodeRequestType DetailedCodeRequestType { get; set; }
    public string DetailedCode { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
