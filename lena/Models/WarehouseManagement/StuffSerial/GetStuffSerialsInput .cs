using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  public class GetStuffSerialsInput : SearchInput<StuffSerialSortType>
  {
    public GetStuffSerialsInput(PagingInput pagingInput, StuffSerialSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public string Serial { get; set; }
    public string[] Serials { get; set; }
    public string CRC { get; set; }
    public int? StuffId { get; set; }
    public int?[] StuffIds { get; set; }
    public int? ProductionOrderId { get; set; }
    public string StuffCode { get; set; }
    public int? SerialProfileCode { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public long? BatchNo { get; set; }
    public int? Order { get; set; }
    public int? InitQty { get; set; }
    public int? InitUnitId { get; set; }
    public System.DateTime? FromDateTime { get; set; }
    public System.DateTime? ToDateTime { get; set; }
    public string FromSerial { get; set; }
    public string ToSerial { get; set; }
    public int? FromOrder { get; set; }
    public int? ToOrder { get; set; }
    public StuffSerialStatus[] Statuses { get; set; }
    public int? CooperatorId { get; set; }
    public string CooperatorCode { get; set; }
    public int? WarehouseId { get; set; }
    public bool? GroupBySerial { get; set; }
    public long? StuffSerialCode { get; set; }
    public string StorReceiptCode { get; set; }
    public bool? GetSerialLastOperationInfo { get; set; }
    public int? Step { get; set; }
  }
}
