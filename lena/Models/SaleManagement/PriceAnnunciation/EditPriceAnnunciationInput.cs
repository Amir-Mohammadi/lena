using lena.Domains.Enums;
using lena.Models.SaleManagement.PriceAnnunciationItem;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PriceAnnunciation
{
  public class EditPriceAnnunciationInput
  {
    public int Id { get; set; }
    public PriceAnnunciationStatus? Status { get; set; }
    public int CooperatorId { get; set; }
    public string FileKey { get; set; }
    public System.DateTime ValidityFromDate { get; set; }
    public System.DateTime? ValidityToDate { get; set; }
    public AddPriceAnnunciationItemInput[] AddPriceAnnunciationItems { get; set; }
    public EditPriceAnnunciationItemInput[] EditPriceAnnunciationItems { get; set; }
    public int[] DeletePriceAnnunciationItems { get; set; }
  }
}