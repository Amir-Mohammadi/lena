using lena.Models.Supplies.StuffBasePriceCustoms;
using lena.Models.Supplies.StuffBasePriceTransport;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffBasePrice
{
  public class EditComputionalStuffBasePriceInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public double MainPrice { get; set; }
    public byte CurrencyId { get; set; }
    public AddStuffBasePriceCustomsInput StuffBasePriceCustoms { get; set; }
    public AddStuffBasePriceTransportInput StuffBasePriceTransport { get; set; }
    public string Description { get; set; }
    public int? PurchaseOrderId { get; set; }
  }
}