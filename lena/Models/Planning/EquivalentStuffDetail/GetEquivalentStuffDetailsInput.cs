using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.EquivalentStuffDetail
{
  public class GetEquivalentStuffDetailsInput : SearchInput<EquivalentStuffDetailSortType>
  {
    public int? EquivalentStuffId { get; set; }

    public GetEquivalentStuffDetailsInput(PagingInput pagingInput, EquivalentStuffDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
