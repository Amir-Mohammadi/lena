using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLineWorkShift
{
  public class GetProductionLineWorkShiftsInput : SearchInput<ProductionLineWorkShiftSortType>
  {
    public int? ProductionLineId { get; set; }

    public GetProductionLineWorkShiftsInput(PagingInput pagingInput, SortOrder sortOrder, ProductionLineWorkShiftSortType sortType) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
