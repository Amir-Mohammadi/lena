using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models
{
  public class GetWarehouseFiscalPeriodsInput : SearchInput<WarehosueFiscalPeriodSortType>
  {
    public GetWarehouseFiscalPeriodsInput(PagingInput pagingInput, WarehosueFiscalPeriodSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
  }
}
