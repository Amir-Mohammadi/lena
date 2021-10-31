using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ProductionYear
{
  public class GetProductionYearsInput : SearchInput<ProductionYearSortType>
  {
    public string Code { get; set; }
    public DateTime? Year { get; set; }
    public GetProductionYearsInput(PagingInput pagingInput, ProductionYearSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

