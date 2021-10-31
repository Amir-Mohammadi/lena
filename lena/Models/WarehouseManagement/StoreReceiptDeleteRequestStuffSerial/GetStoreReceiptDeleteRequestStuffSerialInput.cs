using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceiptDeleteRequestStuffSerial
{
  public class GetStoreReceiptDeleteRequestStuffSerialInput : SearchInput<StoreReceiptDeleteRequestStuffSerialSortType>
  {
    public int StoreReceiptDeleteRequestId { get; set; }

    public GetStoreReceiptDeleteRequestStuffSerialInput(PagingInput pagingInput, StoreReceiptDeleteRequestStuffSerialSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }



  }
}
