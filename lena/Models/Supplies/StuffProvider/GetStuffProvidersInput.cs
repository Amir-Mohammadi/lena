using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffProvider
{
  public class GetStuffProvidersInput : SearchInput<StuffProviderSortType>
  {
    public GetStuffProvidersInput(PagingInput pagingInput, StuffProviderSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? StuffId { get; set; }
    public int? ProviderId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDefault { get; set; }
  }
}
