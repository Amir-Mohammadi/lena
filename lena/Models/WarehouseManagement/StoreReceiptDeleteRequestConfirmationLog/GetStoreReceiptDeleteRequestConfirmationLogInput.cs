using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceiptDeleteRequestStuffSerial
{
  public class GetStoreReceiptDeleteRequestConfirmationLogInput : SearchInput<StoreReceiptDeleteRequestConfirmationLogSortType>
  {
    public int? StoreReceiptDeleteRequestId { get; set; }

    public GetStoreReceiptDeleteRequestConfirmationLogInput(PagingInput pagingInput, StoreReceiptDeleteRequestConfirmationLogSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }



  }
}
