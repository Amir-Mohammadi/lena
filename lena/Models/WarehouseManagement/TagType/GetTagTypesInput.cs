using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TagType
{
  public class GetTagTypesInput : SearchInput<TagTypeSortType>
  {
    public GetTagTypesInput(PagingInput pagingInput, TagTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
