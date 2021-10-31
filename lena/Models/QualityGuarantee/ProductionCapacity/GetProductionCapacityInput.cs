using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityGuarantee.ProductionCapacity
{
  public class GetProductionCapacityInput : SearchInput<ProductionCapacitySortType>
  {
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public double Factor { get; set; }
    public double? StandardWorkTime { get; set; }
    public double ProducedQtyByEachEmployee { get; set; }
    public int[] ProductionLineIds { get; set; }

    public GetProductionCapacityInput(PagingInput pagingInput, ProductionCapacitySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
  }
}