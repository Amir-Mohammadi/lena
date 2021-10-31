using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class GetTestOperationsInput : SearchInput<TestOperationSortType>
  {
    public string Name { get; set; }
    public string Code { get; set; }
    public GetTestOperationsInput(PagingInput pagingInput, TestOperationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
