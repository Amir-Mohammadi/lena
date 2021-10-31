using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseInventory
{
  public class GetWarehouseInventoryForIssueInput : SearchInput<WarehouseInventorySortType>
  {
    public GetWarehouseInventoryForIssueInput(PagingInput pagingInput, WarehouseInventorySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public short WarehouseId { get; set; }
    public string Serial { get; set; }
    public int? SerialProfileCode { get; set; }
    public string[] SelectedSerial { get; set; }
  }
}