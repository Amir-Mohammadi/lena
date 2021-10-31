using lena.Services.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.QualityGuarantee.ProductionCapacity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityGuarantee
{
  public partial class QualityGuarantee
  {
    public IQueryable<ProductionCapacityResult> SearchProductionCapacity(IQueryable<ProductionCapacityResult> query,
      string searchText,
      AdvanceSearchItem[] advanceSearchItems
      )
    {

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }

    public IOrderedQueryable<ProductionCapacityResult> SortProductionCapacityResult(IQueryable<ProductionCapacityResult> query, SortInput<ProductionCapacitySortType> sort)
    {
      switch (sort.SortType)
      {

        case ProductionCapacitySortType.Date:
          return query.OrderBy(a => a.Date, sort.SortOrder);
        case ProductionCapacitySortType.EmployeeCount:
          return query.OrderBy(a => a.EmployeeCount, sort.SortOrder);
        case ProductionCapacitySortType.Capacity:
          return query.OrderBy(a => a.Capacity, sort.SortOrder);
        case ProductionCapacitySortType.ProducedCount:
          return query.OrderBy(a => a.ProducedCount, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
