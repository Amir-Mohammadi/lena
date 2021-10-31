using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.StuffCategory
{
  public class GetStuffCategoryInput : SearchInput<StuffCategorySortType>
  {
    public GetStuffCategoryInput(PagingInput pagingInput, StuffCategorySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
  }
}
