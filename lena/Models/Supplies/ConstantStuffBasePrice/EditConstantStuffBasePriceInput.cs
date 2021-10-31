using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffBasePrice
{
  public class EditConstantStuffBasePriceInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public double Price { get; set; }
    public byte CurrencyId { get; set; }
    public string Description { get; set; }
    public int? PurchaseOrderId { get; set; }

  }
}