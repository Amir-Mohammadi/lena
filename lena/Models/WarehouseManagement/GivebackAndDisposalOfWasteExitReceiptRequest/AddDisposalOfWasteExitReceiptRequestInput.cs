using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.GivebackAndDisposalOfWasteExitReceiptRequest
{
  public class AddDisposalOfWasteExitReceiptRequestInput
  {
    public string Address { get; set; }
    public int CooperatorId { get; set; }
    public string Description { get; set; }
    public int QualityControlId { get; set; }
    public int StuffId { get; set; }
    public byte UnitId { get; set; }
    public short WarehouseId { get; set; }
    public string[] Serials { get; set; }
    public double[] Amounts { get; set; }
    public double UnitPrice { get; set; }
    public byte CurrencyId { get; set; }
  }
}
