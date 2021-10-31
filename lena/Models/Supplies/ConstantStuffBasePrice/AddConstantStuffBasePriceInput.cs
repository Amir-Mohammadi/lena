using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffBasePrice
{
  public class AddConstantStuffBasePriceInput
  {
    //public int StuffId { get; set; }
    public int? StuffPriceId { get; set; }
    public byte[] StuffPriceRowVersion { get; set; }
    public int[] StuffIds { get; set; }
    public double Price { get; set; }
    public byte CurrencyId { get; set; }
    public string Description { get; set; }
    public int? PurchaseOrderId { get; set; }
  }
}