using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models
{
  public class GetCooperatorContactInput : SearchInput<CooperatorContactSortType>
  {
    public bool? IsMain { get; set; }
    public int? CooperatorId { get; set; }
    public GetCooperatorContactInput(PagingInput pagingInput, CooperatorContactSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
