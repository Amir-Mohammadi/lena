using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialDetail
{
  public class GetSerialDetailStuffsInput : SearchInput<SerialDetailStuffsSortType>
  {
    public GetSerialDetailStuffsInput(PagingInput pagingInput, SerialDetailStuffsSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public string Serial { get; set; }
  }
}
