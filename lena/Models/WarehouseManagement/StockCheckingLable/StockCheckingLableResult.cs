using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockCheckingLable
{
  public class StockCheckingLableResult
  {
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int StockCheckingId { get; set; }
    public string StockCheckingTitle { get; set; }
    public string TagTypeName { get; set; }
    public string EmployeeFullName { get; set; }
  }
}
