using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Country
{
  public class GetCountriesInput : SearchInput<CountrySortType>
  {
    public int? Id { get; set; }
    public string Title { get; set; }

    public GetCountriesInput(PagingInput pagingInput, CountrySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
