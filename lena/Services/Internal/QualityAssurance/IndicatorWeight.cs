using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using lena.Models;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Internals.QualityAssurance.Exception;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.QualityAssurance.IndicatorWeight;
// using lena.Services.Core.Foundation.Service.External;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {

    #region AddIndicatorWeightProcess
    public IndicatorWeight AddIndicatorWeightProcess(
       string name,
       string code,
       short? departmentId,
       AddWeightDayInput[] addWeightDayInputs)
    {


      Department department = null;
      if (departmentId != null)
      {
        department = App.Internals.ApplicationBase.GetDepartment(id: departmentId.Value);
      }
      #region
      var indicatorWeight = AddIndicatorWeight(
          name: name,
          department: department,
          code: code);
      #endregion

      #region AddWeightDays
      foreach (var item in addWeightDayInputs)
      {

        AddWeightDay(
                  indicatorWeightId: indicatorWeight.Id,
                  day: item.Day,
                  amount: item.Amount);

      }
      #endregion
      return indicatorWeight;
    }
    #endregion

    #region Add
    public IndicatorWeight AddIndicatorWeight(
       string name,
       Department department,
       string code)
    {


      var indicatorWeight = repository.Create<IndicatorWeight>();
      indicatorWeight.Name = name;
      indicatorWeight.Code = code;
      indicatorWeight.Department = department;
      repository.Add(indicatorWeight);
      return indicatorWeight;
    }
    #endregion

    #region Get       
    public IndicatorWeight GetIndicatorWeight(int id) => GetIndicatorWeight(selector: e => e, id: id);
    public TResult GetIndicatorWeight<TResult>(
        Expression<Func<IndicatorWeight, TResult>> selector,
        int id)
    {

      var indicatorWeight = GetIndicatorWeights(
                selector: selector,
                id: id).FirstOrDefault();
      if (indicatorWeight == null)
        throw new IndicatorWeightNotFoundException(id);
      return indicatorWeight;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetIndicatorWeights<TResult>(
        Expression<Func<IndicatorWeight, TResult>> selector,
        TValue<int> id = null,
        TValue<int> departmentId = null,
        TValue<string> code = null,
        TValue<string> name = null
        )
    {

      var indicatorWeights = repository.GetQuery<IndicatorWeight>();
      if (id != null)
        indicatorWeights = indicatorWeights.Where(m => m.Id == id);
      if (name != null)
        indicatorWeights = indicatorWeights.Where(m => m.Name == name);
      if (code != null)
        indicatorWeights = indicatorWeights.Where(m => m.Code == code);
      if (departmentId != null)
        indicatorWeights = indicatorWeights.Where(m => m.DepartmentId == departmentId);
      return indicatorWeights.Select(selector);
    }
    #endregion

    #region Remove IndicatorWeight
    public void RemoveIndicatorWeight(int id, byte[] rowVersion)
    {

      var indicatorWeight = GetIndicatorWeight(id: id);

    }
    #endregion

    #region Delete IndicatorWeight
    public void DeleteIndicatorWeight(int id)
    {

      var indicatorWeight = GetIndicatorWeight(id: id);

      #region Delete WeightDays
      var weightDayList = indicatorWeight.WeightDays.ToList();
      foreach (var weightDay in weightDayList)
      {
        DeleteWeightDay(weightDay.Id);
      }
      #endregion

      repository.Delete(indicatorWeight);
    }
    #endregion

    #region EditProcess
    public IndicatorWeight EditIndicatorWeightProcess(
        byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<string> code = null,
        TValue<short> departmentId = null,
        AddWeightDayInput[] addWeightDayInputs = null,
        EditWeightDayInput[] editWeightDayInputs = null,
        int[] deleteWeightDayInputs = null
        )
    {

      #region EditIndicatorWeight
      Department department = null;
      if (departmentId != null)
      {
        department = App.Internals.ApplicationBase.GetDepartment(id: departmentId);
      }

      var indicatorWeight = EditIndicatorWeight(
                id: id,
                name: name,
                code: code,
                department: department,
                rowVersion: rowVersion
                );
      #endregion

      #region AddWeightDay
      if (addWeightDayInputs != null)
      {
        foreach (var addWeightDayInput in addWeightDayInputs)
        {
          AddWeightDay(
                    day: addWeightDayInput.Day,
                    amount: addWeightDayInput.Amount,
                    indicatorWeightId: indicatorWeight.Id);
        }
      }
      #endregion

      #region EditWeightDay
      if (editWeightDayInputs != null)
      {
        foreach (var editWeightDayInput in editWeightDayInputs)
        {
          EditWeightDay(
                    id: editWeightDayInput.Id,
                    day: editWeightDayInput.Day,
                    amount: editWeightDayInput.Amount,
                    rowVersion: editWeightDayInput.RowVersion);
        }
      }
      #endregion

      #region DeleteWeightday
      if (deleteWeightDayInputs != null)
      {
        foreach (var deleteWeightDay in deleteWeightDayInputs)
        {
          DeleteWeightDay
                (
                    id: deleteWeightDay);
        }
      }
      #endregion

      return indicatorWeight;
    }
    #endregion

    #region Edit
    public IndicatorWeight EditIndicatorWeight(
        byte[] rowVersion,
        int id,
        Department department,
        TValue<string> code = null,
        TValue<string> name = null)
    {

      var indicatorWeight = GetIndicatorWeight(id: id);
      if (name != null)
        indicatorWeight.Name = name;
      if (code != null)
        indicatorWeight.Code = code;
      indicatorWeight.Department = department;
      if (department != null)
      {
        indicatorWeight.DepartmentId = department.Id;
      }
      else
      {
        indicatorWeight.DepartmentId = null;
      }

      repository.Update(indicatorWeight, rowVersion);
      return indicatorWeight;
    }
    #endregion

    #region Search
    public IQueryable<IndicatorWeightResult> SearchIndicatorWeight(IQueryable<IndicatorWeightResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Name.ToString().Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<IndicatorWeightResult> SortIndicatorWeightResult(IQueryable<IndicatorWeightResult> query,
        SortInput<IndicatorWeightSortType> sort)
    {
      switch (sort.SortType)
      {
        case IndicatorWeightSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case IndicatorWeightSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case IndicatorWeightSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToIndicatorWeightResult
    public Expression<Func<IndicatorWeight, IndicatorWeightResult>> ToIndicatorWeightResult =
        indicatorWeight => new IndicatorWeightResult
        {
          Id = indicatorWeight.Id,
          Name = indicatorWeight.Name,
          Code = indicatorWeight.Code,
          DepartmentId = indicatorWeight.DepartmentId,
          DepartmentName = indicatorWeight.Department.Name,
          WeightDayResults = indicatorWeight.WeightDays.AsQueryable().Select(App.Internals.QualityAssurance.ToWeightDayResult),
          RowVersion = indicatorWeight.RowVersion
        };
    #endregion

    #region ToIndicatorWeightComboList
    public IQueryable<IndicatorWeightComboResult> ToIndicatorWeightComboList(IQueryable<IndicatorWeight> input)
    {
      return from indicatorWeight in input
             select new IndicatorWeightComboResult()
             {
               Id = indicatorWeight.Id,
               Name = indicatorWeight.Name,
               Code = indicatorWeight.Code
             };
    }
    #endregion

  }

}
