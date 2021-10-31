using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffStockPlace
{
  public class GroupedStuffStockPlaceResult
  {
    public int StuffId { get; set; }
    public short WarehouseId { get; set; }
    public IQueryable<string> StockPlaceCodes { get; set; }
    public IQueryable<string> StockPlaceTitles { get; set; }
  }
}
