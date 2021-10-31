using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestItem
{
  public class StuffRequestItemResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public byte[] RowVersion { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public StuffType? StuffType { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public double Qty { get; set; }
    public double AvailableAmount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public string Description { get; set; }
    public bool IsDelete { get; set; }
    public int StuffRequestId { get; set; }
    public double ResponsedQty { get; set; }
    public StuffRequestItemStatusType Status { get; set; }
    public int? BillOfMaterialVersion { get; set; }
  }
}
