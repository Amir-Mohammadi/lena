using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Production.ProductionOperationSequence;
using lena.Models.Production.ProductionOperator;
using lena.Models.Production.ProductionOrder;
using lena.Models.WarehouseManagement.BaseTransaction;
//using System.Data.Entity;
using lena.Services.Internals.Production.Exception;
//using System.Data.Entity.SqlServer;
using lena.Models.Production.ProductionPerformanceInfo;
using lena.Models.Planning.EmployeeOperatorType;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {

    #region ToResult
    public IQueryable<ProductionPerformanceInfoResult> ToProductionPerformanceInfoResult(
       IQueryable<ProductionPerformanceInfo> productionPerformanceInfoes)
    {
      var resultQuery = from productionPerformanceInfo in productionPerformanceInfoes
                        select new ProductionPerformanceInfoResult
                        {
                          Id = productionPerformanceInfo.Id,
                          RowVersion = productionPerformanceInfo.RowVersion,
                          DescriptionDateTime = productionPerformanceInfo.DescriptionDateTime,
                          Description = productionPerformanceInfo.Description,
                          ResponsibleComment = productionPerformanceInfo.ResponsibleComment,
                          CorrectiveAction = productionPerformanceInfo.CorrectiveAction,
                          Status = productionPerformanceInfo.Status.Value,
                          DepartmentId = productionPerformanceInfo.DepartmentId,
                          ProductionOrderId = productionPerformanceInfo.ProductionOrderId,
                          DepartmentFullName = productionPerformanceInfo.Department.Name,
                          RegistratorUserName = productionPerformanceInfo.RegistratorUser.Employee.FirstName + " " + productionPerformanceInfo.RegistratorUser.Employee.LastName,
                          ProductionOrderQty = productionPerformanceInfo.ProductionOrder.Qty,
                          ProductionOrderLineName = productionPerformanceInfo.ProductionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionLine.Name,
                          ProductionOrderStuffName = productionPerformanceInfo.ProductionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Stuff.Name,
                          ProductionOrderProducedQty = productionPerformanceInfo.ProductionOrder.ProductionOrderSummary.ProducedQty,
                          ConfirmatorUserName = productionPerformanceInfo.ConfirmatorUser.Employee.FirstName + " " + productionPerformanceInfo.ConfirmatorUser.Employee.LastName,

                        };
      return resultQuery;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProductionPerformanceInfoes<TResult>(
       Expression<Func<ProductionPerformanceInfo, TResult>> selector,
       TValue<int> id = null,
       TValue<int> productionOrderId = null
    )
    {

      var query = repository.GetQuery<ProductionPerformanceInfo>();
      if (id != null)
        query = query.Where(a => a.Id == id);
      if (productionOrderId != null)
        query = query.Where(a => a.ProductionOrderId == productionOrderId);
      return query.Select(selector);
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionPerformanceInfoesResult<TResult>(
       Expression<Func<ProductionPerformanceInfo, TResult>> selector,
       TValue<int> id = null,
         TValue<ProductionPerformanceInfoStatus> status = null,
         TValue<DateTime> dateTime = null
      )
    {

      var query = repository.GetQuery<ProductionPerformanceInfo>();
      if (id != null)
        query = query.Where(a => a.Id == id);
      if (status != null)
        query = query.Where(a => a.Status == status);
      if (dateTime != null)
        query = query.Where(a => a.DescriptionDateTime == dateTime);
      return query.Select(selector);
    }
    #endregion
    #region Search
    public IQueryable<ProductionPerformanceInfoResult> SearchProductionPerformaneInfo(IQueryable<ProductionPerformanceInfoResult>
    query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems
    )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.RegistratorUserName.Contains(searchText) ||
            item.DepartmentFullName.Contains(searchText) ||
            item.Description.Contains(searchText) ||
            item.ResponsibleComment.Contains(searchText) ||
            item.ProductionOrderStuffName.Contains(searchText) ||
            item.ProductionOrderLineName.Contains(searchText)
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
    public IOrderedQueryable<ProductionPerformanceInfoResult> SortProductionPerformanceInfoResult(IQueryable<ProductionPerformanceInfoResult> query,
        SortInput<ProductionPerformanceInfoSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProductionPerformanceInfoSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ProductionPerformanceInfoSortType.DescriptionDateTime:
          return query.OrderBy(a => a.DescriptionDateTime, sort.SortOrder);
        case ProductionPerformanceInfoSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case ProductionPerformanceInfoSortType.DepartmentFullName:
          return query.OrderBy(a => a.DepartmentFullName, sort.SortOrder);
        case ProductionPerformanceInfoSortType.CorrectiveAction:
          return query.OrderBy(a => a.CorrectiveAction, sort.SortOrder);
        case ProductionPerformanceInfoSortType.ProductionOrderLineName:
          return query.OrderBy(a => a.ProductionOrderLineName, sort.SortOrder);
        case ProductionPerformanceInfoSortType.ProductionOrderProducedQty:
          return query.OrderBy(a => a.ProductionOrderProducedQty, sort.SortOrder);
        case ProductionPerformanceInfoSortType.ProductionOrderQty:
          return query.OrderBy(a => a.ProductionOrderQty, sort.SortOrder);
        case ProductionPerformanceInfoSortType.ProductionOrderStuffName:
          return query.OrderBy(a => a.ProductionOrderStuffName, sort.SortOrder);
        case ProductionPerformanceInfoSortType.RegistratorUserName:
          return query.OrderBy(a => a.RegistratorUserName, sort.SortOrder);
        case ProductionPerformanceInfoSortType.ResponsibleComment:
          return query.OrderBy(a => a.ResponsibleComment, sort.SortOrder);
        case ProductionPerformanceInfoSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Add
    public ProductionPerformanceInfo AddProductionPerformanceInfo(
        string description,
        int productionOrderId,
        DateTime? dateTime
    )
    {

      var productionPerformanceInfo = repository.Create<ProductionPerformanceInfo>();
      productionPerformanceInfo.Description = description;
      productionPerformanceInfo.DescriptionDateTime = dateTime;
      productionPerformanceInfo.ProductionOrderId = productionOrderId;
      productionPerformanceInfo.RegistratorUserId = App.Providers.Security.CurrentLoginData.UserId;
      productionPerformanceInfo.RegistrationDate = DateTime.UtcNow;
      productionPerformanceInfo.Status = ProductionPerformanceInfoStatus.None;
      repository.Add(productionPerformanceInfo);
      return productionPerformanceInfo;
    }
    #endregion
    #region Edit
    public ProductionPerformanceInfo EditProductionPerformanceInfo(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<DateTime> dateTime = null,
        TValue<string> responsibleComment = null,
        TValue<string> correctiveAction = null,
        TValue<ProductionPerformanceInfoStatus> status = null,
        TValue<short> departmentId = null,
        TValue<bool> descriptionMode = null

        )
    {

      var productionPerformanceInfo = GetProductionPerformanceInfo(id: id);
      if (description != null)
        productionPerformanceInfo.Description = description;
      if (dateTime != null)
        productionPerformanceInfo.DescriptionDateTime = dateTime;
      if (responsibleComment != null)
        productionPerformanceInfo.ResponsibleComment = responsibleComment;
      if (correctiveAction != null)
        productionPerformanceInfo.CorrectiveAction = correctiveAction;
      if (status != null)
        productionPerformanceInfo.Status = status;
      if (departmentId != null)
        productionPerformanceInfo.DepartmentId = departmentId;
      if (descriptionMode == true)
      {
        productionPerformanceInfo.RegistratorUserId = App.Providers.Security.CurrentLoginData.UserId;
        productionPerformanceInfo.RegistrationDate = DateTime.UtcNow;
      }
      else
      {
        productionPerformanceInfo.ConfirmatorUserId = App.Providers.Security.CurrentLoginData.UserId;
        productionPerformanceInfo.ConfirmationDate = DateTime.UtcNow;
      }

      repository.Update(rowVersion: rowVersion, entity: productionPerformanceInfo);
      return productionPerformanceInfo;
    }
    #endregion

    #region Get 
    public ProductionPerformanceInfo GetProductionPerformanceInfo(int id) => GetProductionPerformanceInfo(selector: e => e, id: id);
    public TResult GetProductionPerformanceInfo<TResult>(
        Expression<Func<ProductionPerformanceInfo, TResult>> selector,
        int id)
    {

      var productionPerformanceInfo = GetProductionPerformanceInfoes(selector: selector, id: id)


            .FirstOrDefault();
      if (productionPerformanceInfo == null)
        throw new ProductionPerformanceInfoNotFoundException(id);
      return productionPerformanceInfo;
    }
    #endregion
  }
}
