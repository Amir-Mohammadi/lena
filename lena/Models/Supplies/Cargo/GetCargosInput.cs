using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Cargo
{
  public class GetCargosInput : SearchInput<CargoSortType>
  {
    public GetCargosInput(PagingInput pagingInput, CargoSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int?[] Ids { get; set; }
    public string Code { get; set; }
    public int? HowToBuyId { get; set; }
    public int? StuffId { get; set; }
    public int? HowToBuyDetailId { get; set; }
  }
}
