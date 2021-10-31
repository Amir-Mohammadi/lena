using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.Provider
{
  public class GetProvidersInput : SearchInput<ProviderSortType>
  {

    public GetProvidersInput(PagingInput pagingInput, ProviderSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}