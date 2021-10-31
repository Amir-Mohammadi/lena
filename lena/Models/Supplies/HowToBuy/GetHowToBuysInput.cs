using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.HowToBuy
{
  public class GetHowToBuysInput : SearchInput<HowToBuySortType>
  {
    public GetHowToBuysInput(PagingInput pagingInput, HowToBuySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
