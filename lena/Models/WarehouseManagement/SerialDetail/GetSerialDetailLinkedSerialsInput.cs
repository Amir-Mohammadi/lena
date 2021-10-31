using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialDetail
{
  public class GetSerialDetailLinkedSerialsInput : SearchInput<SerialDetailLinkedSerialSortType>
  {
    public GetSerialDetailLinkedSerialsInput(PagingInput pagingInput, SerialDetailLinkedSerialSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public string Serial { get; set; }
    public int[] StuffIds { get; set; }
  }
}
