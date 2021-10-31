using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class GetCargoItemLogInput : SearchInput<CargoItemLogSortType>
  {
    public int? CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public DateTime? ToModifyDateTime { get; set; }
    public DateTime? FromModifyDateTime { get; set; }
    public int? StuffId { get; set; }
    public int? ModifierEmployeeId { get; set; }
    public GetCargoItemLogInput(PagingInput pagingInput, CargoItemLogSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
