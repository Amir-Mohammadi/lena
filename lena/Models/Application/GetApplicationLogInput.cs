using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Application
{
  public class GetApplicationLogsInput : SearchInput<ApplicationLogSortType>
  {
    public int? UserId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }

    public GetApplicationLogsInput(PagingInput pagingInput, ApplicationLogSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
  }
}
