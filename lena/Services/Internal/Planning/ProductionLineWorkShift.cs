using System;
using System.Collections.Generic;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Planning.ProductionLineWorkShift;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<ProductionLineWorkShift> GetProductionLineWorkShifts(
        TValue<int> id = null,
        TValue<int> productionLineId = null,
        TValue<int> workShiftId = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> saveDate = null)
    {

      var isIdNull = id == null;
      var isProductionLineIdNull = productionLineId == null;
      var isWorkShiftIdNull = workShiftId == null;
      var isFromDateNull = fromDate == null;
      var isSaveDateNull = saveDate == null;
      var workStationWorkShifts = from item in repository.GetQuery<ProductionLineWorkShift>()
                                  where (isIdNull || item.Id == id) &&
                                              (isProductionLineIdNull || item.ProductionLineId == productionLineId) &&
                                              (isWorkShiftIdNull || item.WorkShiftId == workShiftId) &&
                                              (isFromDateNull || item.FromDate == fromDate) &&
                                              (isSaveDateNull || item.SaveDate == saveDate)
                                  select item;
      return workStationWorkShifts;
    }

    public ProductionLineWorkShift GetProductionLineWorkShift(int id)
    {

      var productionLineWorkShift = GetProductionLineWorkShifts(id: id).SingleOrDefault();
      if (productionLineWorkShift == null)
        throw new ProductionLineWorkShiftNotFoundException(id);
      return productionLineWorkShift;

    }

    public ProductionLineWorkShift AddProductionLineWorkShift(
        int productionLineId,
        int workShiftId,
        DateTime fromDate,
        DateTime saveDate)
    {

      var productionLineWorkShift = repository.Create<ProductionLineWorkShift>();
      productionLineWorkShift.ProductionLineId = productionLineId;
      productionLineWorkShift.WorkShiftId = workShiftId;
      productionLineWorkShift.FromDate = fromDate;
      productionLineWorkShift.SaveDate = saveDate;
      repository.Add(productionLineWorkShift);
      return productionLineWorkShift;
    }

    public IQueryable<ProductionLineWorkShiftResult> ToProductionLineWorkShiftResultQuery(IQueryable<ProductionLineWorkShift> query)
    {
      var resultQuery = from workStationWorkShift in query
                        select new ProductionLineWorkShiftResult()
                        {
                          Id = workStationWorkShift.Id,
                          SaveDate = workStationWorkShift.SaveDate,
                          WorkShiftId = workStationWorkShift.WorkShiftId,
                          WorkShiftName = workStationWorkShift.WorkShift.Name,
                          ProductionLineId = workStationWorkShift.ProductionLineId,
                          ProductionLineName = workStationWorkShift.ProductionLine.Name,
                          FromDate = workStationWorkShift.FromDate,
                          RowVersion = workStationWorkShift.RowVersion
                        };
      return resultQuery;
    }
    public IQueryable<ProductionLineWorkTimeResult> ToProductionLineWorkTimeResultQuery(IQueryable<ProductionLineWorkShift> query)
    {
      var resultQuery = from productionLineWorkShift in query
                        let productionLine = productionLineWorkShift.ProductionLine
                        let workShift = productionLineWorkShift.WorkShift
                        from workTime in workShift.CalendarEvents
                        select new ProductionLineWorkTimeResult()
                        {
                          ProductionLineId = productionLineWorkShift.ProductionLineId,
                          ProductionLineName = productionLine.Name,
                          WorkShiftName = workShift.Name,
                          WorkShiftId = productionLineWorkShift.WorkShiftId,
                          DateTime = workTime.DateTime,
                          Duration = workTime.Duration,
                          RowVersion = workTime.RowVersion
                        };
      return resultQuery;
    }

    public IOrderedQueryable<ProductionLineWorkTimeResult> SortProductionLineWorkTimeResult(IQueryable<ProductionLineWorkTimeResult> input, SortInput<ProductionLineWorkTimeSortType> options)
    {
      switch (options.SortType)
      {
        case ProductionLineWorkTimeSortType.DateTime:
          return input.OrderBy(i => i.DateTime, options.SortOrder);
        case ProductionLineWorkTimeSortType.ToDateTime:
          return input.OrderBy(i => i.ToDateTime, options.SortOrder);
        case ProductionLineWorkTimeSortType.Duration:
          return input.OrderBy(i => i.Duration, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IQueryable<ProductionLineWorkTimeResult> SearchProductionLineWorkTimeResultQuery(
        IQueryable<ProductionLineWorkTimeResult> query, DateTime? fromDate, DateTime? toDate)
    {
      if (fromDate != null)
        query = query.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.DateTime <= toDate);
      return query;
    }

    public ProductionLineWorkShiftResult ToProductionLineWorkShiftResult(ProductionLineWorkShift workStationWorkShift)
    {
      var result = new ProductionLineWorkShiftResult()
      {
        Id = workStationWorkShift.Id,
        FromDate = workStationWorkShift.FromDate,
        ProductionLineId = workStationWorkShift.ProductionLineId,
        ProductionLineName = workStationWorkShift.ProductionLine.Name,
        WorkShiftId = workStationWorkShift.WorkShiftId,
        WorkShiftName = workStationWorkShift.WorkShift.Name,
        SaveDate = workStationWorkShift.SaveDate,
        RowVersion = workStationWorkShift.RowVersion
      };
      return result;
    }

    public IOrderedQueryable<ProductionLineWorkShiftResult> SortProductionLineWorkShiftResult(IQueryable<ProductionLineWorkShiftResult> input, SortInput<ProductionLineWorkShiftSortType> options)
    {
      switch (options.SortType)
      {
        case ProductionLineWorkShiftSortType.FromDate:
          return input.OrderBy(i => i.FromDate, options.SortOrder);
        case ProductionLineWorkShiftSortType.SaveDate:
          return input.OrderBy(i => i.SaveDate, options.SortOrder);
        case ProductionLineWorkShiftSortType.WorkShiftName:
          return input.OrderBy(i => i.WorkShiftName, options.SortOrder);
        case ProductionLineWorkShiftSortType.WorkStationName:
          return input.OrderBy(i => i.ProductionLineName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IQueryable<ProductionLineWorkShiftResult> SearchProductionLineWorkShift(
       IQueryable<ProductionLineWorkShiftResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems,
       int? productionLineId = null
       )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from productionLineWorkShift in query
                where productionLineWorkShift.WorkShiftName.Contains(searchText) ||
                      productionLineWorkShift.ProductionLineName.Contains(searchText)
                select productionLineWorkShift;
      }

      if (productionLineId.HasValue)
      {
        query = query.Where(i => i.ProductionLineId == productionLineId);
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
  }
}
