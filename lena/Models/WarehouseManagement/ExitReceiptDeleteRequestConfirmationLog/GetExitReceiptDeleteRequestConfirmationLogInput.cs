using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceiptDeleteRequestConfirmationLog
{
  public class GetExitReceiptDeleteRequestConfirmationLogInput : SearchInput<ExitReceiptDeleteRequestConfirmationLogSortType>
  {
    public int? ExitReceiptDeleteRequestId { get; set; }

    public GetExitReceiptDeleteRequestConfirmationLogInput(PagingInput pagingInput, ExitReceiptDeleteRequestConfirmationLogSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }



  }
}
