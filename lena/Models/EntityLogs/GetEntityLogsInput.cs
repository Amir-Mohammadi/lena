using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.EntityLogs
{
  public class GetEntityLogsInput : SearchInput<EntityLogSortType>
  {
    public GetEntityLogsInput(PagingInput pagingInput, EntityLogSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? UserId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

  }
}
