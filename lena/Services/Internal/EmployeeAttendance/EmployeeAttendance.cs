//using LinqLib.Sort;
using lena.Services.Common;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.EmployeeAttendance;
using lena.Models.QualityGuarantee.ProductionCapacity;
using System;
// using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.EmployeeAttendance
{
  public partial class EmployeeAttendance
  {
    public IQueryable<EmployeeWorkDetailView> GetEmployeeWorkDetail(
      TValue<DateTime> date = null,
      TValue<DateTime> fromDateTime = null,
      TValue<DateTime> toDateTime = null,
      TValue<string> employeeCode = null,
      TValue<string[]> employeeCodes = null)
    {
      //Data loaded from OfficeAutomation (192.168.0.5) and date, data not saved in utc format
      DateTime? fromDate = fromDateTime != null ? fromDateTime.Value.ToLocalTime() : (DateTime?)null;
      DateTime? toDate = toDateTime != null ? toDateTime.Value.ToLocalTime() : (DateTime?)null;


      var query = repository.GetQuery<EmployeeWorkDetailView>();
      if (date != null)
        query = query.Where(i => i.Date == date);

      if (fromDate != null)
        query = query.Where(i => i.Date >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.Date <= toDate);
      if (employeeCode != null)
        query = query.Where(i => i.EmployeeCode == employeeCode);
      if (employeeCodes != null)
        query = query.Where(i => employeeCodes.Value.Contains(i.EmployeeCode));

      return query;
    }

    public IOrderedQueryable<EmployeeWorkDetailResult> SortEmployeeWorkDetailResult(IQueryable<EmployeeWorkDetailResult> query, SortInput<EmployeeWorkDetailSortType> sort)
    {
      switch (sort.SortType)
      {
        case EmployeeWorkDetailSortType.Date:
          return query.OrderBy(i => i.Date, sort.SortOrder);
        case EmployeeWorkDetailSortType.UtcDate:
          return query.OrderBy(i => i.UtcDate, sort.SortOrder);
        case EmployeeWorkDetailSortType.EmployeeCode:
          return query.OrderBy(i => i.EmployeeCode, sort.SortOrder);
        case EmployeeWorkDetailSortType.EmployeeFullName:
          return query.OrderBy(i => i.EmployeeFullName, sort.SortOrder);
        case EmployeeWorkDetailSortType.FirstEnterTime:
          return query.OrderBy(i => i.FirstEnterTime, sort.SortOrder);
        case EmployeeWorkDetailSortType.LastExitTime:
          return query.OrderBy(i => i.LastExitTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException($"{nameof(EmployeeWorkDetailSortType)} is not implemented!");
      }
    }
  }
}
