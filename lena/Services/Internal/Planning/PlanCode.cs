using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Planning.PlanCode;
using System.Linq.Expressions;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Add
    public PlanCode AddPlanCode(
        string code
    )
    {
      var planCode = repository.Create<PlanCode>();
      planCode.Code = code;
      planCode.IsActive = true;
      planCode.RegisterarUserId = App.Providers.Security.CurrentLoginData.UserId;
      planCode.RegisterDateTime = DateTime.UtcNow;
      repository.Add(planCode);
      return planCode;
    }
    #endregion
    #region Edit
    public PlanCode EditPlanCode(byte[] rowVersion, int id, TValue<string> code = null, TValue<bool> isActive = null
        )
    {
      var planCode = GetPlanCode(id: id);
      if (code != null)
        planCode.Code = code;
      if (isActive != null)
        planCode.IsActive = isActive;
      repository.Update(rowVersion: planCode.RowVersion, entity: planCode);
      return planCode;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetPlanCodes<TResult>(
         Expression<Func<PlanCode, TResult>> selector,
         TValue<int> id = null,
         TValue<string> code = null,
         TValue<bool> isActive = null)
    {
      var baseQuery = repository.GetQuery<PlanCode>();
      if (id != null)
        baseQuery = baseQuery.Where(i => i.Id == id);
      if (code != null)
        baseQuery = baseQuery.Where(i => i.Code == code);
      if (isActive != null)
        baseQuery = baseQuery.Where(i => i.IsActive == isActive);
      return baseQuery.Select(selector);
    }
    #endregion
    #region ToResult
    public IQueryable<PlanCodeResult> ToPlanCodeResultQuery(
       IQueryable<PlanCode> planCodes
       )
    {
      var resultQuery = from planCode in planCodes
                        select new PlanCodeResult
                        {
                          Id = planCode.Id,
                          Code = planCode.Code,
                          IsActive = planCode.IsActive,
                          RowVersion = planCode.RowVersion,
                          RegisterarUserName = planCode.RegisterarUser.Employee.FirstName + " " + planCode.RegisterarUser.Employee.LastName,
                          RegisterDateTime = planCode.RegisterDateTime
                        };
      return resultQuery;
    }
    #endregion
    #region Search
    public IQueryable<PlanCodeResult> SearchPlanCode(IQueryable<PlanCodeResult>
    query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems
    )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Code.Contains(searchText) ||
            item.RegisterarUserName.Contains(searchText)
            );
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<PlanCodeResult> SortPlanCodeResult(IQueryable<PlanCodeResult> query,
        SortInput<PlanCodeSortType> sort)
    {
      switch (sort.SortType)
      {
        case PlanCodeSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case PlanCodeSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case PlanCodeSortType.IsActive:
          return query.OrderBy(a => a.IsActive, sort.SortOrder);
        case PlanCodeSortType.RegisterarUserName:
          return query.OrderBy(a => a.RegisterarUserName, sort.SortOrder);
        case PlanCodeSortType.RegisterDateTime:
          return query.OrderBy(a => a.RegisterDateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Get
    public PlanCode GetPlanCode(int id)
    {
      var planeCode = GetPlanCodes(selector: e => e, id: id);
      if (planeCode == null)
        throw new PlanCodeNotFoundException(id: id);
      return planeCode;
    }
    #endregion
    #region Delete
    public void DeletePlanCode(int id)
    {
      var purchaseRequest = App.Internals.Supplies.GetPurchaseRequests(selector: e => e, planCodeId: id);
      if (purchaseRequest != null)
      {
        throw new PlanCodeUsedInPurchaseRequestException(id: id);
      }
      var planeCode = GetPlanCode(id);
      repository.Delete(planeCode);
    }
    #endregion
    #region ComboToResult
    public IQueryable<PlanCodeComboResult> ToPlanCodesComboResultQuery(IQueryable<PlanCode> query)
    {
      return from planCode in query
             select new PlanCodeComboResult
             {
               Id = planCode.Id,
               Code = planCode.Code,
             };
    }
    #endregion
  }
}