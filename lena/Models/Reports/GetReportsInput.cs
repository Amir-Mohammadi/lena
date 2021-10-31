using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Reports
{
  public class GetReportsInput : SearchInput<ReportSortType>
  {
    public string Name { get; set; }
    public object Params { get; set; }

    public GetReportsInput(PagingInput pagingInput, ReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
