using System;
using System.Linq;
using lena.Services.Common;
using lena.Models.Common;
using lena.Domains;
using lena.Models;
using System.Linq.Expressions;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Internals.QualityAssurance.Exception;
//using lena.Services.Core.Foundation.Service.Internal.Action;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    #region Add
    public WeightDay AddWeightDay(
       int day,
       int indicatorWeightId,
       double amount)
    {

      var weightDayRes = GetWeightDays(e => e).Where((m) => m.Day == day && m.IndicatorWeightId == indicatorWeightId);
      if (weightDayRes.Any())
        throw new HasTheSameDayInWeightDayException(day);

      var weightDay = repository.Create<WeightDay>();
      weightDay.Day = day;
      weightDay.Amount = amount;
      weightDay.IndicatorWeightId = indicatorWeightId;
      repository.Add(weightDay);
      return weightDay;
    }
    #endregion

    #region Get
    public WeightDay GetWeightDay(int id) => GetWeightDay(selector: e => e, id: id);
    public TResult GetWeightDay<TResult>(
        Expression<Func<WeightDay, TResult>> selector,
        int id)
    {

      var weightDay = GetWeightDays(
                selector: selector,
                id: id).FirstOrDefault();
      if (weightDay == null)
        throw new WeightDayNotFoundException(id);
      return weightDay;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetWeightDays<TResult>(
        Expression<Func<WeightDay, TResult>> selector,
        TValue<int> id = null,
        TValue<double> amount = null,
        TValue<int> indicatorWeightId = null,
        TValue<string> indicatorWeightCode = null
        )
    {

      var weightDays = repository.GetQuery<WeightDay>();
      if (id != null)
        weightDays = weightDays.Where(m => m.Id == id);
      if (amount != null)
        weightDays = weightDays.Where(m => m.Amount == amount);
      if (indicatorWeightId != null)
        weightDays = weightDays.Where(m => m.IndicatorWeightId == indicatorWeightId);
      if (indicatorWeightCode != null)
        weightDays = weightDays.Where(m => m.IndicatorWeight.Code == indicatorWeightCode);
      return weightDays.Select(selector);
    }
    #endregion

    #region Remove WeightDay
    public void RemoveWeightDay(int id, byte[] rowVersion)
    {

      var weightDay = GetWeightDay(id: id);

    }
    #endregion

    #region Delete WeightDay
    public void DeleteWeightDay(int id)
    {

      var weightDay = GetWeightDay(id: id);
      repository.Delete(weightDay);
    }
    #endregion

    #region EditProcess
    public WeightDay EditWeightDay(
        byte[] rowVersion,
        int id,
        TValue<int> day = null,
        TValue<double> amount = null)
    {

      var weightDay = GetWeightDay(id: id);
      if (amount != null)
        weightDay.Amount = amount;
      if (day != null)
        weightDay.Day = day;

      repository.Update(weightDay, rowVersion);
      return weightDay;
    }
    #endregion

    #region Search
    public IQueryable<WeightDayResult> SearchWeightDay(IQueryable<WeightDayResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems

        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Amount.ToString().Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<WeightDayResult> SortWeightDayResult(IQueryable<WeightDayResult> query,
        SortInput<WeightDaySortType> sort)
    {
      switch (sort.SortType)
      {
        case WeightDaySortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case WeightDaySortType.Amount:
          return query.OrderBy(a => a.Amount, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToWeightDayResult
    public Expression<Func<WeightDay, WeightDayResult>> ToWeightDayResult =
        weightDay => new WeightDayResult
        {
          Id = weightDay.Id,
          Day = weightDay.Day,
          Amount = weightDay.Amount,
          RowVersion = weightDay.RowVersion
        };
    #endregion


  }

}
