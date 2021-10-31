using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseInventory
{
  public class GetWarehouseInventoriesInput : SearchInput<WarehouseInventorySortType>
  {
    public GetWarehouseInventoriesInput(PagingInput pagingInput, WarehouseInventorySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? WarehouseId { get; set; }
    public int? StuffCategoryId { get; set; }
    public int? StuffId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public bool GroupByBillOfMaterialVersion { get; set; }
    public string Serial { get; set; }
    public bool GroupBySerial { get; set; }
    public DateTime? FromEffectDateTime { get; set; }
    public DateTime? ToEffectDateTime { get; set; }
    public StuffSerialStatus[] SerialStatuses { get; set; }
  }
}