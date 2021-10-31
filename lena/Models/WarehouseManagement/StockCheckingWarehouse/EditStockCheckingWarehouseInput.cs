using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockCheckingWarehouse
{
  public class EditStockCheckingWarehouseInput : StockCheckingWarehouseSelectorInput
  {
    public byte[] RowVersion { get; set; }
  }
}
