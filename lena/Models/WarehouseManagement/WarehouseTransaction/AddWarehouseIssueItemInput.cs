using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseTransaction
{
  public class AddWarehouseIssueItemInput
  {
    public int StuffId { get; set; }
    public string Serial { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public string AssetCode { get; set; }
    public string Description { get; set; }
  }
}
