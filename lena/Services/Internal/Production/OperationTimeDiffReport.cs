using lena.Services.Common;
using lena.Services.Core;
using Microsoft.EntityFrameworkCore;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Production.OperationTimeDiffReport;
using lena.Models.Supplies.PurchaseReport;
using System;
using System.Collections.Generic;
//using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Gets
    public IQueryable<OperationTimeDiffReportResult> GetOperationTimeDiffReports(
        int[] employeeIds,
        TValue<int> stuffId,
        TValue<int> operationId,
        TValue<int> productionLineId,
        TValue<DateTime> fromWorkDate,
        TValue<DateTime> toWorkDate)
    {

      var productionOperations = App.Internals.Production.GetProductionOperations(
                selector: e => e,
                stuffId: stuffId,
                operationId: operationId,
                productionLineId: productionLineId,
                fromDateTime: fromWorkDate,
                toDateTime: toWorkDate);

      var productionOperationEmployees = App.Internals.Production.GetProductionOperationEmployees(
                e => e,
                employeeIds: employeeIds);

      var minQuery = from po in productionOperations
                     join poe in productionOperationEmployees on po.ProductionOperationEmployeeGroupId equals poe.ProductionOperationEmployeeGroupId
                     select new
                     {
                       Id = po.Id,
                       EmployeeId = poe.EmployeeId,
                       ProductionOperationDateTime = po.DateTime,
                     };

      var groupQuery = from q1 in minQuery
                       from q2 in minQuery
                       where q2.ProductionOperationDateTime < q1.ProductionOperationDateTime && q1.EmployeeId == q2.EmployeeId
                       group q2 by q1.Id into gItems
                       select new
                       {
                         Id = gItems.Key,
                         PreviousProductionOperationDateTime = gItems.Max(i => i.ProductionOperationDateTime),
                       };

      var result = from po in productionOperations
                   join poe in productionOperationEmployees on po.ProductionOperationEmployeeGroupId equals poe.ProductionOperationEmployeeGroupId
                   join pdate in groupQuery on po.Id equals pdate.Id
                   select new OperationTimeDiffReportResult()
                   {
                     Id = po.Id,
                     UserCode = poe.Employee.Code,
                     UserFullName = poe.Employee.FirstName + poe.Employee.LastName,
                     FirstOperationTime = pdate.PreviousProductionOperationDateTime,
                     SecondOperationTime = po.DateTime,
                     OperationDifferenceTime = EF.Functions.DateDiffSecond(pdate.PreviousProductionOperationDateTime, po.DateTime),
                   };


      return result;

    }

    #endregion
    #region Sort
    public IOrderedQueryable<OperationTimeDiffReportResult> SortOperationTimeDiffReportResult(
        IQueryable<OperationTimeDiffReportResult> query,
        SortInput<OperationTimeDiffReportSortType> sort)

    {
      switch (sort.SortType)
      {
        case OperationTimeDiffReportSortType.FirstOperationTime:
          return query.OrderBy(a => a.FirstOperationTime, sort.SortOrder);
        case OperationTimeDiffReportSortType.SecondOperationTime:
          return query.OrderBy(a => a.SecondOperationTime, sort.SortOrder);
        case OperationTimeDiffReportSortType.OperationDifferenceTime:
          return query.OrderBy(a => a.OperationDifferenceTime, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<OperationTimeDiffReportResult> SearchOperationTimeDiffReportResult(
        IQueryable<OperationTimeDiffReportResult> query,
        string searchText,
         AdvanceSearchItem[] advanceSearchItems,
         DateTime? fromWorkDate,
         DateTime? toWorkDate)

    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.UserCode.Contains(searchText) ||
                 item.UserFullName.Contains(searchText)
                select item;

      if (fromWorkDate != null)
        query = query.Where(i => i.FirstOperationTime >= fromWorkDate);
      if (toWorkDate != null)
        query = query.Where(i => i.SecondOperationTime <= toWorkDate);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }

    #endregion
  }
}