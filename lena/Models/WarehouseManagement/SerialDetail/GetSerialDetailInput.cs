using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialDetail
{
  public class GetSerialDetailInput : SearchInput<SerialDetailSortType>
  {
    public GetSerialDetailInput(PagingInput pagingInput, SerialDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public string Serial { get; set; }
    public int[] StuffIds { get; set; }
  }
}
