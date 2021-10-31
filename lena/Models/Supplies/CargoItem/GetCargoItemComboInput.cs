using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class GetCargoItemComboInput : SearchInput<CargoItemComboSortType>
  {
    public string Code { get; set; }
    public string CargoCode { get; set; }
    public GetCargoItemComboInput(PagingInput pagingInput, CargoItemComboSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
