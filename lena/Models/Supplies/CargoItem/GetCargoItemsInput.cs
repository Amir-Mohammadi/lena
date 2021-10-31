using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class GetCargoItemsInput : SearchInput<CargoItemSortType>
  {
    public int? CargoId { get; set; }
    public int? CargoItemId { get; set; }
    public int[] CargoItemIds { get; set; }
    public int? StuffId { get; set; }
    public int? CooperatorId { get; set; }

    public GetCargoItemsInput(PagingInput pagingInput, CargoItemSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
