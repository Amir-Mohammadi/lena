using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionTerminal
{
  public class GetProductionTerminalsInput : SearchInput<ProductionTerminalSortType>
  {
    public int? ProductionLineId { get; set; }
    public GetProductionTerminalsInput(PagingInput pagingInput, ProductionTerminalSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

  }
}
