using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialTracking
{

  public class GetSerialOperationsTrackingInput : SearchInput<SerialOperationsTrackingSortType>
  {
    public GetSerialOperationsTrackingInput(PagingInput pagingInput, SerialOperationsTrackingSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public string Serial { get; set; }
  }

}
