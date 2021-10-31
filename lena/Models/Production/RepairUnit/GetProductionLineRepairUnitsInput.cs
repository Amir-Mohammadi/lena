using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Production.RepairUnit
{
  public class GetProductionLineRepairUnitsInput : SearchInput<ProductionLineRepairUnitSortType>
  {
    public GetProductionLineRepairUnitsInput(PagingInput pagingInput, ProductionLineRepairUnitSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? Id { get; set; }
    public int? ProductionLineId { get; set; }
    public int? WarehouseId { get; set; }
    public string Name { get; set; }
    public int? UserId { get; set; }
  }
}
