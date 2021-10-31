using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Printers
{
  public class GetPrinterInput : SearchInput<PrinterSortTypes>
  {
    public GetPrinterInput(PagingInput pagingInput, PrinterSortTypes sortType, SortOrder sortOrder) : base(
        pagingInput, sortType, sortOrder)
    {
    }
  }
}
