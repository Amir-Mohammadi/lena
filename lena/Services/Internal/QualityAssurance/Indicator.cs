using System;
using System.Linq;
using lena.Domains;
using lena.Services.Common;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using lena.Models;
using lena.Models.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.QualityAssurance.Indicator;
using lena.Services.Internals.Exceptions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    #region Add
    public Indicator AddIndicator(
        string name,
        string type,
        double target,
        double weight,
        int apiInfoId,
        string formula,
        short departmentId,
        string description)
    {

      var indicator = repository.Create<Indicator>();
      indicator.Name = name;
      indicator.Type = type;
      indicator.Target = target;
      indicator.Weight = weight;
      indicator.ApiInfoId = apiInfoId;
      indicator.DepartmentId = departmentId;
      indicator.Description = description;
      indicator.Formula = formula;
      repository.Add(indicator);
      return indicator;
    }
    #endregion

    #region Get
    public Indicator GetIndicator(int id) => GetIndicator(selector: e => e, id: id);
    public TResult GetIndicator<TResult>(
        Expression<Func<Indicator, TResult>> selector,
        int id)
    {

      var indicator = GetIndicators(
                selector: selector,
                id: id).FirstOrDefault();
      if (indicator == null)
        throw new IndicatorNotFoundException(id);
      return indicator;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetIndicators<TResult>(
        Expression<Func<Indicator, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> type = null,
        TValue<int> apiInfoId = null,
        TValue<string> formula = null,
        TValue<int> departmentId = null
        )
    {

      var indicators = repository.GetQuery<Indicator>();
      if (id != null)
        indicators = indicators.Where(x => x.Id == id);
      if (name != null)
        indicators = indicators.Where(i => i.Name == name);
      if (type != null)
        indicators = indicators.Where(i => i.Type == type);
      if (apiInfoId != null)
        indicators = indicators.Where(i => i.ApiInfoId == apiInfoId);
      if (formula != null)
        indicators = indicators.Where(i => i.Formula == formula);
      if (departmentId != null)
        indicators = indicators.Where(i => i.DepartmentId == departmentId);
      return indicators.Select(selector);
    }
    #endregion

    #region Remove Indicator
    public void RemoveIndicator(int id, byte[] rowVersion)
    {

      var indicator = GetIndicator(id: id);

    }
    #endregion

    #region Delete Indicator
    public void DeleteIndicator(int id)
    {

      var indicator = GetIndicator(id: id);
      repository.Delete(indicator);
    }
    #endregion

    #region EditProcess
    public Indicator EditIndicator(
         byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<string> type = null,
        TValue<int> apiInfoId = null,
        TValue<double> target = null,
        TValue<double> weight = null,
        TValue<string> formula = null,
        TValue<short> departmentId = null,
        TValue<string> description = null)
    {

      var indicator = GetIndicator(id: id);
      if (name != null)
        indicator.Name = name;
      if (type != null)
        indicator.Type = type;
      if (target != null)
        indicator.Target = target;
      if (weight != null)
        indicator.Weight = weight;
      if (apiInfoId != null)
        indicator.ApiInfoId = apiInfoId;
      if (formula != null)
        indicator.Formula = formula;
      if (departmentId != null)
        indicator.DepartmentId = departmentId;
      if (description != null)
        indicator.Description = description;

      repository.Update(indicator, rowVersion);
      return indicator;
    }
    #endregion

    #region Search
    public IQueryable<IndicatorResult> SearchIndicator(IQueryable<IndicatorResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems

        )
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Name.Contains(searchText));

      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region SortCombo
    public IOrderedQueryable<IndicatorComboResult> SortIndicatorComboResult(
        IQueryable<IndicatorComboResult> query, SortInput<IndicatorSortType> type)
    {
      switch (type.SortType)
      {
        case IndicatorSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case IndicatorSortType.Name:
          return query.OrderBy(a => a.Name, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Sort
    public IOrderedQueryable<IndicatorResult> SortIndicatorResult(IQueryable<IndicatorResult> query,
        SortInput<IndicatorSortType> sort)
    {
      switch (sort.SortType)
      {
        case IndicatorSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case IndicatorSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToIndicatorResult
    public Expression<Func<Indicator, IndicatorResult>> ToIndicatorResult =
        indicator => new IndicatorResult
        {
          Id = indicator.Id,
          Name = indicator.Name,
          Type = indicator.Type,
          Target = indicator.Target,
          Weight = indicator.Weight,
          ApiInfoId = indicator.ApiInfoId,
          ApiInfoUrl = indicator.ApiInfo.Url,
          ApiInfoParam = indicator.ApiInfo.Param,
          ApiInfoSortTypeName = indicator.ApiInfo.SortTypeName,
          ApiInfoSortTypeFieldName = indicator.ApiInfo.SortTypeFieldName,
          Formula = indicator.Formula,
          Description = indicator.Description,
          DepartmentId = indicator.DepartmentId,
          DepartmentName = indicator.Department.Name,
          RowVersion = indicator.RowVersion
        };
    #endregion

    #region ToIndicatorComboResult
    public Expression<Func<Indicator, IndicatorComboResult>> ToIndicatorComboResult =
        indicator => new IndicatorComboResult
        {
          Id = indicator.Id,
          Name = indicator.Name

        };
    #endregion
  }

}
