using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.StuffDocument
{
  public class GetStuffDocumentInput : SearchInput<StuffDocumentSortType>
  {
    public GetStuffDocumentInput(PagingInput pagingInput, StuffDocumentSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int StuffId { get; set; }
  }
}
