using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockCheckingLable
{
  public class GetStockCheckingLableInput
  {
    public string WarehouseName { get; set; }
    public string StockCheckingTitle { get; set; }
    public string TagTypeName { get; set; }
    public string EmployeeFullName { get; set; }
    public int CountLabel { get; set; }

  }
}
