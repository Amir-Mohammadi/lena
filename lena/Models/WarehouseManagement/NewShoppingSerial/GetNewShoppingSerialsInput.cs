using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.NewShoppingSerial
{
  public class GetNewShoppingSerialsInput : SearchInput<NewShoppingSerialSortType>
  {
    public int? StoreReceiptId { get; set; }
    public string Serial { get; set; }
    public int? StuffId { get; set; }
    public GetNewShoppingSerialsInput(PagingInput pagingInput, NewShoppingSerialSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
