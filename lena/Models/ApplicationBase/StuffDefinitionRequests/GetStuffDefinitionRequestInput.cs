using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.StuffDefinitionRequests
{
  public class GetStuffDefinitionRequestInput : SearchInput<StuffDefinitionRequestSortType>
  {
    public GetStuffDefinitionRequestInput(PagingInput pagingInput, StuffDefinitionRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public string StuffCode { get; set; }
    public StuffDefinitionStatus? DefinitionStatus { get; set; }
    public StuffType? StuffType { get; set; }
  }
}
