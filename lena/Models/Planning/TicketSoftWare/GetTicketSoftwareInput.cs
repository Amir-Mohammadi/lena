using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Planning.TicketSoftware
{
  public class GetTicketSoftwareInput : SearchInput<TicketSoftwareSortType>
  {
    public GetTicketSoftwareInput(PagingInput pagingInput, TicketSoftwareSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public DateTime? FromCreateDateTime { get; set; }
    public DateTime? ToCreateDateTime { get; set; }

  }
}
