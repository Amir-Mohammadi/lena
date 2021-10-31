using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PriceAnnunciationItem
{
  public class AddPriceAnnunciationItemInput
  {
    public int StuffId { get; set; }
    public double Price { get; set; }
    public byte CurrencyId { get; set; }
    public double? Count { get; set; }
    public int PriceAnnunciationId { get; set; }
  }
}
