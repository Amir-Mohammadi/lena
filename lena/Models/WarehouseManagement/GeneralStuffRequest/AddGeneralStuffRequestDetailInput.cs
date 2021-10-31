using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.GeneralStuffRequest
{
  public class AddGeneralStuffRequestDetailInput
  {
    public int GeneralStuffRequestId { get; set; }
    public int? StuffRequestId { get; set; }
    public int? PurchaseRequestId { get; set; }
    public int? AlternativePurchaseRequestId { get; set; }
    public int? GeneralPurchaseRequestId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string Description { get; set; }
  }
}
