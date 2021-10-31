using lena.Domains.Enums;
namespace lena.Models.Supplies.ProvisionersCartItemDetail
{
  public class EditProvisionersCartItemDetailInput
  {
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public double SupplyQty { get; set; }
    public byte[] RowVersion { get; set; }
    public int ProvisionersCartItemId { get; set; }
    public int UnitPrice { get; set; }
    public int CurrencyId { get; set; }
  }
}