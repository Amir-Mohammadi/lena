using lena.Models.Supplies.StuffBasePriceCustoms;
using lena.Models.Supplies.StuffBasePriceTransport;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffBasePrice
{
  public class AddComputionalStuffBasePriceInput
  {
    //public int StuffId { get; set; }
    public int StuffPriceId { get; set; }
    public byte[] StuffPriceRowVersion { get; set; }
    public int[] StuffIds { get; set; }
    public double MainPrice { get; set; }
    public byte CurrencyId { get; set; }
    public string Description { get; set; }
    public int? PurchaseOrderId { get; set; }
    public AddStuffBasePriceCustomsInput StuffBasePriceCustoms { get; set; }
    public AddStuffBasePriceTransportInput StuffBasePriceTransport { get; set; }
  }
}