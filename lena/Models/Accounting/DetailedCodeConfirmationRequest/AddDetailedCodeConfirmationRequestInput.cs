using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.DetailedCodeConfirmationRequest
{
  public class AddDetailedCodeConfirmationRequestInput
  {
    public int? CooperatorId { get; set; }
    public int? ProductionLineId { get; set; }
    public DetailedCodeEntityType DetailedCodeEntityType { get; set; }
    public DetailedCodeRequestType DetailedCodeRequestType { get; set; }
    public string Description { get; set; }
    public string DetailedCode { get; set; }
  }
}
