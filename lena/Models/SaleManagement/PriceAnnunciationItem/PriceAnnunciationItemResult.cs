using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PriceAnnunciationItem
{
  public class PriceAnnunciationItemResult
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int StuffId { get; set; }
    public double PriceAnnunciation { get; set; }
    public double? TotalPrice { get; set; }
    public string CurrencyName { get; set; }
    public int CurrencyId { get; set; }
    public double? Count { get; set; }
    public PriceAnnunciationItemStatus Status { get; set; }
    public string Description { get; set; }
    public string ConfirmerUserName { get; set; }
    public System.DateTime? ConfirmationDateTime { get; set; }
  }
}
