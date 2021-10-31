using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.QtyCorrectionRequest
{
  public class GetQtyCorrectionRequestInventoriesInput : SearchInput<QtyCorrectionRequestInventorySortType>
  {
    public GetQtyCorrectionRequestInventoriesInput(PagingInput pagingInput, QtyCorrectionRequestInventorySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public int? WarehouseId { get; set; }
    public string Serial { get; set; }
    public QtyCorrectionRequestType[] Types { get; set; }
    public QtyCorrectionRequestStatus[] Statuses { get; set; }
  }
}
