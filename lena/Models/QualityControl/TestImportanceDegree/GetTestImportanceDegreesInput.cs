using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class GetTestImportanceDegreesInput : SearchInput<TestImportanceDegreeSortType>
  {
    public string Name { get; set; }
    public GetTestImportanceDegreesInput(PagingInput pagingInput, TestImportanceDegreeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
