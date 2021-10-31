using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PriceAnnunciationItem
{
  public class PriceAnnunciationItemComboResult
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public double Price { get; set; }
    public int CooperatorId { get; set; }
    public string CurrencyName { get; set; }
  }
}
