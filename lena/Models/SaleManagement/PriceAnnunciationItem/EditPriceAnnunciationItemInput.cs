using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PriceAnnunciationItem
{
  public class EditPriceAnnunciationItemInput
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public PriceAnnunciationItemStatus? Status { get; set; }
    public double? Price { get; set; }
    public byte? CurrencyId { get; set; }
    public int StuffId { get; set; }
    public double? Count { get; set; }
    public int? PriceAnnunciationId { get; set; }
  }
}
