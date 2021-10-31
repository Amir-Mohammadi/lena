using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums.SortTypes;
using lena.Domains.Enums;
using lena.Models.Common;
using System;


using lena.Domains.Enums;
namespace lena.Models.Production.ProductionPerformanceInfo
{
  public class GetProductionPerformaneInfoesInput : SearchInput<ProductionPerformanceInfoSortType>
  {
    public int Id { get; set; }
    public Nullable<System.DateTime> DescriptionDateTime { get; set; }
    public Nullable<ProductionPerformanceInfoStatus> Status { get; set; }
    public GetProductionPerformaneInfoesInput(PagingInput pagingInput, ProductionPerformanceInfoSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
