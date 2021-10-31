using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ReturnOfSaleSerial
{
  public class GetReturnOfSaleSerialsInput : SearchInput<ReturnOfSaleSerialSortType>
  {
    public string Serial { get; set; }
    public int? StuffId { get; set; }
    public int ReturnStoreReceiptId { get; set; }
    public GetReturnOfSaleSerialsInput(PagingInput pagingInput, ReturnOfSaleSerialSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
