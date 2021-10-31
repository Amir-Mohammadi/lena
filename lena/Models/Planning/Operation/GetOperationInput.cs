using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.Operation
{
  public class GetOperationInput : SearchInput<OperationSortType>
  {
    public string Code { get; set; }
    public string[] Codes { get; set; }
    public bool? IsQualityControl { get; set; }
    public bool? IsCorrective { get; set; }

    public GetOperationInput(PagingInput pagingInput, OperationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
