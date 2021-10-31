using lena.Models.SaleManagement.PriceAnnunciationItem;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PriceAnnunciation
{
  public class AddPriceAnnunciationInput
  {
    public int CooperatorId { get; set; }
    public string FileKey { get; set; }
    public System.DateTime ValidityFromDate { get; set; }
    public System.DateTime? ValidityToDate { get; set; }
    public AddPriceAnnunciationItemInput[] PriceAnnunciationItems { get; set; }
  }
}