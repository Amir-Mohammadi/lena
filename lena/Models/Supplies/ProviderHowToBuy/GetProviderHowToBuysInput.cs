using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.ProviderHowToBuy
{
  public class GetProviderHowToBuysInput : SearchInput<ProviderHowToBuySortType>
  {
    public GetProviderHowToBuysInput(PagingInput pagingInput, ProviderHowToBuySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? ProviderId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDefault { get; set; }
    public short? HowToBuyId { get; set; }
  }
}
