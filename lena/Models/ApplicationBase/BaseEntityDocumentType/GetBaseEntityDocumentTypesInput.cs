using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.BaseEntityDocumentType
{
  public class GetBaseEntityDocumentTypesInput : SearchInput<BaseEntityDocumentTypeSortType>
  {
    public EntityType? EntityType { get; set; }
    public string Title { get; set; }

    public GetBaseEntityDocumentTypesInput(PagingInput pagingInput, BaseEntityDocumentTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

  }
}
