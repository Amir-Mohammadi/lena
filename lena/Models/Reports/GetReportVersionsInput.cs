using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Reports
{
  public class GetReportVersionsInput : SearchInput<ReportVersionSortType>
  {
    public GetReportVersionsInput(PagingInput pagingInput, ReportVersionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
