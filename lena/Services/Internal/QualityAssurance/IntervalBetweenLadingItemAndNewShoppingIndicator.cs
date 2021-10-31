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

    #region Gets IntervalBetweenLadingItemAndNewShoppingIndicator
    public IQueryable<IntervalBetweenLadingItemAndNewShoppingIndicatorResult> GetIntervalBetweenLadingItemAndNewShoppingIndicators(
        int indicatorWeightId,
        DateTime fromDateTime,
        DateTime toDateTime)
    {


      #region GetNewShoppings
      var newShoppings = App.Internals.WarehouseManagement.GetNewShoppings(
          e => e,
          isDelete: false,
          toDateTime: toDateTime,
          fromDateTime: fromDateTime,
          providerType: ProviderType.Foreign);
      #endregion

      #region GetWeightDays
      var weightDays = GetWeightDays(
           e => e,
           indicatorWeightId: indicatorWeightId);
      #endregion

      var intervalQuery = from newShopping in newShoppings
                          select new
                          {
                            LadingItemId = newShopping.LadingItem.Id,
                            NewShoppingId = newShopping.Id,
                            LadingItemDateTime = newShopping.LadingItem.DateTime,
                            NewShoppingDateTime = newShopping.DateTime,
                            DelayDay = EF.Functions.DateDiffDay(newShopping.LadingItem.DateTime, newShopping == null ? (DateTime?)null : newShopping.DateTime)
                          };

      var groupResult = from interval in intervalQuery
                        group interval by interval.DelayDay into g
                        select new
                        {
                          DelayDay = g.Key.Value,
                          DelayDayTemp = g.Key.Value > 16 ? 17 : g.Key.Value,
                          NewShoppingIds = g.Select(s => s.NewShoppingId),
                          NewShoppingCount = g.Count(),
                        };

      var result = from item in groupResult
                   join weightDay in weightDays
                         on item.DelayDayTemp equals weightDay.Day
                   select new
                   {
                     DelayDay = item.DelayDay,
                     WeightDay = weightDay.Amount,
                     NewShoppingCount = item.NewShoppingCount,
                     NewShoppingIds = item.NewShoppingIds,
                     Percentage = item.NewShoppingCount * weightDay.Amount,
                     TimelyCheck = item.DelayDayTemp <= 7 ? true : false
                   };

      var weightTimely1 = result.Any() ? result.Where(d => d.DelayDay <= 7)?.Sum(r => r.WeightDay) : 0;
      var weightNotTimely1 = result.Any() ? result.Where(d => d.DelayDay > 7)?.Sum(r => r.WeightDay) : 0;
      if (weightTimely1 == 0 || weightNotTimely1 == 0)
        throw new DivideByZeroInIndicatorReportException();

      var intervalResult = from res in result
                           select new IntervalBetweenLadingItemAndNewShoppingIndicatorResult
                           {
                             Amount = 0d,
                             DelayDay = res.DelayDay,
                             WeightDay = res.WeightDay,
                             NewShoppingIds = res.NewShoppingIds,
                             NewShoppingCount = res.NewShoppingCount,
                             //Percentage = res.Percentage,
                             Percentage = res.DelayDay <= 7 ? (res.Percentage / weightTimely1) : (res.Percentage / weightNotTimely1),
                             TimelyCheck = res.TimelyCheck
                           };


      return intervalResult;
    }
    #endregion

    #region Get IntervalBetweenLadingItemAndNewShoppingIndicator  
    public IntervalBetweenLadingItemAndNewShoppingIndicatorResult GetIntervalBetweenLadingItemAndNewShoppingIndicator(
    TValue<DateTime> fromDate = null,
    TValue<DateTime> toDate = null)
    {

      IQueryable<IntervalBetweenLadingItemAndNewShoppingIndicatorResult> result = Enumerable.Empty<IntervalBetweenLadingItemAndNewShoppingIndicatorResult>().AsQueryable();

      var newShoppings = App.Internals.WarehouseManagement.GetNewShoppings(
                e => e,
                toDateTime: toDate,
                fromDateTime: fromDate,
                providerType: ProviderType.Foreign);

      #region GetWeightDays
      var indicatorWeightCode = SettingKey.ILNIW.ToString(); // شناسه وزن های مربوط به فاصله زمانی بارنامه تا تحویل به شرکت
      var weightDays = GetWeightDays(
               e => e,
               indicatorWeightCode: indicatorWeightCode);

      if (!weightDays.Any())
        throw new WeightDayNotFoundException(code: indicatorWeightCode);
      #endregion

      var intervalQuery = from newShopping in newShoppings
                          select new
                          {
                            LadingItemId = newShopping.LadingItem.Id,
                            NewShoppingId = newShopping.Id,
                            LadingItemDateTime = newShopping.LadingItem.DateTime,
                            NewShoppingDateTime = newShopping.DateTime,
                            DelayDay = EF.Functions.DateDiffDay(newShopping.LadingItem.DateTime, newShopping == null ? (DateTime?)null : newShopping.DateTime)
                          };

      var groupResult = from interval in intervalQuery
                        group interval by interval.DelayDay into g
                        select new
                        {
                          DelayDay = g.Key.Value > 16 ? 17 : g.Key.Value,
                          NewShoppingIds = g.Select(m => m.NewShoppingId),
                          NewShoppingCount = g.Count(),
                        };

      result = from item in groupResult
               join weightDay in weightDays
                     on item.DelayDay equals weightDay.Day
               select new IntervalBetweenLadingItemAndNewShoppingIndicatorResult
               {
                 DelayDay = item.DelayDay,
                 WeightDay = weightDay.Amount,
                 NewShoppingCount = item.NewShoppingCount,
                 Percentage = item.NewShoppingCount * weightDay.Amount
               };

      var sumWeights = result.Any() ? result.Sum(m => m.WeightDay) : 0;

      var intervalResult = new IntervalBetweenLadingItemAndNewShoppingIndicatorResult
      {
        Amount = sumWeights != 0 ? (((double)(result.Sum(m => m.Percentage) / sumWeights))) : 0
      };

      return intervalResult;
    }
    #endregion

    #region Search
    public IQueryable<IntervalBetweenLadingItemAndNewShoppingIndicatorResult> SearchIntervalBetweenLadingItemAndNewShoppingIndicatorResult(
    IQueryable<IntervalBetweenLadingItemAndNewShoppingIndicatorResult> query,
    string search,
    AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.DelayDay.ToString().Contains(search)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }

    #endregion

    #region Sort
    public IOrderedQueryable<IntervalBetweenLadingItemAndNewShoppingIndicatorResult> SortIntervalBetweenLadingItemAndNewShoppingIndicatorResult(
    IQueryable<IntervalBetweenLadingItemAndNewShoppingIndicatorResult> query,
    SortInput<IntervalBetweenLadingItemAndNewShoppingIndicatorSortType> sort)
    {
      switch (sort.SortType)
      {
        case IntervalBetweenLadingItemAndNewShoppingIndicatorSortType.DelayDay:
          return query.OrderBy(a => a.DelayDay, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

  }

}
