using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.EntityConfirmation
{
  public class GetBaseEntityConfirmTypeInput : SearchInput<BaseEntityConfirmTypeSortType>
  {
    public int? id { get; set; } = null;
    public EntityType? confirmType { get; set; } = null;
    public int? departmentId { get; set; } = null;
    public int? userId { get; set; } = null;
    public string confirmPageUrl { get; set; } = null;
    public GetBaseEntityConfirmTypeInput(PagingInput pagingInput, BaseEntityConfirmTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
