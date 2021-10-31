using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.EnactmentActionProcess
{
  public class GetEnactmentActionProcessesInput : SearchInput<EnactmentActionProcessSortType>
  {
    public GetEnactmentActionProcessesInput(PagingInput pagingInput, EnactmentActionProcessSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public string Code { get; set; }
    public string Name { get; set; }
  }
}
