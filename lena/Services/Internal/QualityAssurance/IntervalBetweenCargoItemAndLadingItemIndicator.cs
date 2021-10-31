using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity.SqlServer;
using lena.Models.Common;
using lena.Services.Internals.QualityAssurance.Exception;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    #region Gets IntervalBetweenCargoItemAndLadingItemIndicator
    public IQueryable<IntervalBetweenCargoItemAndLadingItemIndicatorResult> GetIntervalBetweenCargoItemAndLadingItemIndicators(
        int indicatorWeightId,
        DateTime fromDateTime,
        DateTime toDateTime)
    {
      IQueryable<IntervalBetweenCargoItemAndLadingItemIndicatorResult> result = Enumerable.Empty<IntervalBetweenCargoItemAndLadingItemIndicatorResult>().AsQueryable();
      #region GetLadingItem
      var ladingItems = App.Internals.Supplies.GetLadingItems(
          e => e,
          isDelete: false,
          providerType: ProviderType.Foreign,
          fromDateTime: fromDateTime,
          toDateTime: toDateTime)
      .Where((m) => m.Qty != 0);
      #endregion
      #region WeightDays
      var weightDays = GetWeightDays(
          e => e,
          indicatorWeightId: indicatorWeightId);
      #endregion
      var intervalQuery = from ladingItem in ladingItems
                          select new
                          {
                            CargoItemId = ladingItem.CargoItem.Id,
                            LadingItemId = ladingItem.Id,
                            CargoItemDateTime = ladingItem.CargoItem.DateTime,
                            LadingItemDateTime = ladingItem.DateTime,
                            DelayDay = EF.Functions.DateDiffDay(ladingItem.CargoItem.DateTime, ladingItem == null ? (DateTime?)null : ladingItem.DateTime)
                          };
      var groupResult = from interval in intervalQuery
                        group interval by interval.DelayDay into g
                        select new
                        {
                          DelayDayTemp = g.Key.Value > 16 ? 17 : g.Key.Value,
                          DelayDay = g.Key.Value,
                          LadingItemIds = g.Select(m => m.LadingItemId),
                          LadingItemCount = g.Count(),
                        };
      result = from item in groupResult
               join weightDay in weightDays
                     on item.DelayDayTemp equals weightDay.Day
               select new IntervalBetweenCargoItemAndLadingItemIndicatorResult
               {
                 Amount = 0d,
                 DelayDay = item.DelayDay,
                 WeightDay = weightDay.Amount,
                 LadingItemIds = item.LadingItemIds,
                 LadingItemCount = item.LadingItemCount,
                 Percentage = item.LadingItemCount * weightDay.Amount,
                 TimelyCheck = item.DelayDayTemp <= 4 ? true : false
               };
      var weightTimely = result.Any() ? result.Where(m => m.DelayDay <= 4)?.Sum(r => r.WeightDay) : 0;
      var weightNotTimely = result.Any() ? result.Where(m => m.DelayDay > 4)?.Sum(r => r.WeightDay) : 0;
      var intervalResult = from res in result
                           select new IntervalBetweenCargoItemAndLadingItemIndicatorResult
                           {
                             Amount = 0d,
                             DelayDay = res.DelayDay,
                             WeightDay = res.WeightDay,
                             LadingItemIds = res.LadingItemIds,
                             LadingItemCount = res.LadingItemCount,
                             Percentage = res.DelayDay <= 4 ? ((weightTimely == 0) ? 0 : (res.Percentage / weightTimely)) : ((weightNotTimely == 0) ? 0 : (res.Percentage / weightNotTimely)),
                             TimelyCheck = res.TimelyCheck
                           };
      return intervalResult;
    }
    #endregion
    #region Get GetIntervalBetweenCargoItemAndLadingItemIndicator  
    public IntervalBetweenCargoItemAndLadingItemIndicatorResult GetIntervalBetweenCargoItemAndLadingItemIndicator(
    TValue<DateTime> fromDate = null,
    TValue<DateTime> toDate = null)
    {
      #region GetLadingItems
      var ladingItems = App.Internals.Supplies.GetLadingItems(
          e => e,
          isDelete: false,
          providerType: ProviderType.Foreign,
          fromDateTime: fromDate,
          toDateTime: toDate)
      .Where(m => m.Qty != 0);
      #endregion
      #region WeightDays
      var indicatorWeightCode = SettingKey.ICLIW.ToString(); // شناسه وزن های مربوط به فاصله زمانی محموله تا بارنامه               
      var weightDays = GetWeightDays(
          e => e,
          indicatorWeightCode: indicatorWeightCode);
      if (!weightDays.Any())
        throw new WeightDayNotFoundException(code: indicatorWeightCode);
      #endregion
      var intervalQuery = from ladingItem in ladingItems
                          select new
                          {
                            CargoItemId = ladingItem.CargoItem.Id,
                            LadingItemId = ladingItem.Id,
                            CargoItemDateTime = ladingItem.CargoItem.DateTime,
                            LadingItemDateTime = ladingItem.DateTime,
                            DelayDay = EF.Functions.DateDiffDay(ladingItem.CargoItem.DateTime, ladingItem == null ? (DateTime?)null : ladingItem.DateTime)
                          };
      var groupResult = from interval in intervalQuery
                        group interval by interval.DelayDay into g
                        select new
                        {
                          DelayDay = g.Key.Value > 16 ? 17 : g.Key.Value,
                          LadingItemIds = g.Select(m => m.LadingItemId),
                          LadingItemCount = g.Count(),
                        };
      var result = from item in groupResult
                   join weightDay in weightDays
                         on item.DelayDay equals weightDay.Day
                   select new IntervalBetweenCargoItemAndLadingItemIndicatorResult
                   {
                     DelayDay = item.DelayDay,
                     WeightDay = weightDay.Amount,
                     LadingItemCount = item.LadingItemCount,
                     Percentage = item.LadingItemCount * weightDay.Amount
                   };
      var sumWeights = result.Any() ? result.Sum(m => m.WeightDay) : 0;
      var intervalResult = new IntervalBetweenCargoItemAndLadingItemIndicatorResult
      {
        Amount = sumWeights != 0 ? (((double)(result.Sum(m => m.Percentage) / sumWeights))) : 0
      };
      return intervalResult;
    }
    #endregion
    #region
    public IQueryable<IntervalBetweenCargoItemAndLadingItemIndicatorResult> SearchIntervalBetweenCargoItemAndLadingItemIndicatorResult(
    IQueryable<IntervalBetweenCargoItemAndLadingItemIndicatorResult> query,
    string search,
    AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.DelayDay.ToString().Contains(search)
                select item;
      if (advanceSearchItems.Any())
      {
        bool hasProperty = true;
        foreach (var advanceSearchItem in advanceSearchItems)
        {
          if (query.GetType().GetProperty(advanceSearchItem.FieldName) == null)
            hasProperty = false;
        }
        if (hasProperty == true)
          query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region
    public IOrderedQueryable<IntervalBetweenCargoItemAndLadingItemIndicatorResult> SortIntervalBetweenCargoItemAndLadingItemIndicatorResult(
    IQueryable<IntervalBetweenCargoItemAndLadingItemIndicatorResult> query,
    SortInput<IntervalBetweenCargoItemAndLadingItemIndicatorSortType> sort)
    {
      switch (sort.SortType)
      {
        case IntervalBetweenCargoItemAndLadingItemIndicatorSortType.DelayDay:
          return query.OrderBy(a => a.DelayDay, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}