using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Planning.EmployeeOperatorType;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Get
    public EmployeeOperatorType GetEmployeeOperatorType(int employeeId,
        int operatorTypeId) => GetEmployeeOperatorType(
        selector: e => e,
        employeeId: employeeId,
        operatorTypeId: operatorTypeId);
    public TResult GetEmployeeOperatorType<TResult>(
        Expression<Func<EmployeeOperatorType, TResult>> selector,
        int employeeId,
        int operatorTypeId)
    {

      var employeeOperatorType = GetEmployeeOperatorTypes(
                    selector: selector,
                    employeeId: employeeId,
                    operatorTypeId: operatorTypeId)


                .FirstOrDefault();
      if (employeeOperatorType == null)
        throw new EmployeeOperatorTypeNotFoundException(employeeId: employeeId,
                  operatorTypeId: employeeId);
      return employeeOperatorType;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetEmployeeOperatorTypes<TResult>(
            Expression<Func<EmployeeOperatorType, TResult>> selector,
            TValue<int> employeeId = null,
            TValue<int> operatorTypeId = null,
            TValue<int[]> employeeIds = null,
            TValue<short[]> operatorTypeIds = null)
    {

      var query = repository.GetQuery<EmployeeOperatorType>();
      if (employeeId != null)
        query = query.Where(x => x.EmployeeId == employeeId);
      if (operatorTypeId != null)
        query = query.Where(x => x.OperatorTypeId == operatorTypeId);
      if (employeeIds != null)
        query = query.Where(x => employeeIds.Value.Contains(x.EmployeeId));
      if (operatorTypeIds != null)
        query = query.Where(x => operatorTypeIds.Value.Contains(x.OperatorTypeId));
      return query.Select(selector);
    }
    #endregion
    #region Add
    public EmployeeOperatorType AddEmployeeOperatorType(
        int employeeId,
        short operatorTypeId)
    {

      var employeeOperatorType = repository.Create<EmployeeOperatorType>();
      employeeOperatorType.EmployeeId = employeeId;
      employeeOperatorType.OperatorTypeId = operatorTypeId;
      repository.Add(employeeOperatorType);
      return employeeOperatorType;
    }
    #endregion
    #region Add EmployeeOperatorTypes Process
    public void AddEmployeeOperatorTypesProcess(
        int[] employeeIds,
        short[] operatorTypeIds)
    {

      #region EmployeeIdOperatorTypeIds
      var employeeIdOperatorTypeIds =
          from employeeId in employeeIds
          from operatorTypeId in operatorTypeIds
          select new
          {
            employeeId,
            operatorTypeId
          };
      #endregion
      #region  Get ExistEmployeeOperatorTypes
      var existEmployeeOperatorTypes = GetEmployeeOperatorTypes(
              selector: e => e,
              operatorTypeIds: operatorTypeIds)


          .ToList();
      #endregion
      #region Add 
      foreach (var employeeIdOperatorTypeId in employeeIdOperatorTypeIds)
      {
        var isExist = existEmployeeOperatorTypes.Any(i =>
                  i.EmployeeId == employeeIdOperatorTypeId.employeeId &&
                  i.OperatorTypeId == employeeIdOperatorTypeId.operatorTypeId);
        if (!isExist)
          AddEmployeeOperatorType(
                        employeeId: employeeIdOperatorTypeId.employeeId,
                        operatorTypeId: employeeIdOperatorTypeId.operatorTypeId);
      }
      #endregion
    }
    #endregion
    #region Delete
    public void DeleteEmployeeOperatorType(
        int employeeId,
        int operatorTypeId)
    {

      var employeeOperatorType = GetEmployeeOperatorType(
                    employeeId: employeeId,
                    operatorTypeId: operatorTypeId);
      repository.Delete(employeeOperatorType);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<EmployeeOperatorTypeResult> SortEmployeeOperatorTypeResult(
        IQueryable<EmployeeOperatorTypeResult> query,
        SortInput<EmployeeOperatorTypeSortType> sort)
    {
      switch (sort.SortType)
      {
        case EmployeeOperatorTypeSortType.OperatorTypeName:
          return query.OrderBy(a => a.OperatorTypeName, sort.SortOrder);
        case EmployeeOperatorTypeSortType.EmployeeCode:
          return query.OrderBy(a => a.EmployeeCode, sort.SortOrder);
        case EmployeeOperatorTypeSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<EmployeeOperatorTypeResult> SearchEmployeeOperatorTypeResult(
        IQueryable<EmployeeOperatorTypeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                    item.EmployeeCode.Contains(searchText) ||
                    item.EmployeeFullName.Contains(searchText) ||
                    item.OperatorTypeName.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<EmployeeOperatorType, EmployeeOperatorTypeResult>> ToEmployeeOperatorTypeResult =
                employeeOperatorType => new EmployeeOperatorTypeResult
                {
                  OperatorTypeId = employeeOperatorType.OperatorTypeId,
                  OperatorTypeName = employeeOperatorType.OperatorType.Name,
                  EmployeeId = employeeOperatorType.EmployeeId,
                  EmployeeCode = employeeOperatorType.Employee.Code,
                  EmployeeFullName = employeeOperatorType.Employee.FirstName + " " + employeeOperatorType.Employee.LastName,
                  RowVersion = employeeOperatorType.RowVersion
                };
    #endregion
  }
}
