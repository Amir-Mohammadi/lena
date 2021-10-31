using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceiptDeleteRequestStuffSerial
{
  public class GetExitReceiptDeleteRequestStuffSerialInput : SearchInput<ExitReceiptDeleteRequestStuffSerialSortType>
  {
    public int ExitReceiptDeleteRequestId { get; set; }

    public GetExitReceiptDeleteRequestStuffSerialInput(PagingInput pagingInput, ExitReceiptDeleteRequestStuffSerialSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }



  }
}
