using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.GeneralStuffRequest
{
  public class GeneralStuffRequestDetailResult
  {
    public int Id { get; set; }
    public int GeneralStuffRequestId { get; set; }
    public int? StuffRequestId { get; set; }
    public string StuffRequestCode { get; set; }
    public int? PurchaseRequestId { get; set; }
    public string PurchaseRequestCode { get; set; }
    public int? AlternativePurchaseRequestId { get; set; }
    public string AlternativePurchaseRequestCode { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
