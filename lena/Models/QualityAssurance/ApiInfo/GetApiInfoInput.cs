using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.ApiInfo
{
  public class GetApiInfoInput : SearchInput<ApiInfoSortType>
  {
    public string Name { get; set; }
    public GetApiInfoInput(PagingInput pagingInput, ApiInfoSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}