using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Reports.ReportPrintSettings
{
  public class GetReportPrintSettingsInput : SearchInput<ReportPrintSettingSortType>
  {
    public GetReportPrintSettingsInput(PagingInput pagingInput, ReportPrintSettingSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
