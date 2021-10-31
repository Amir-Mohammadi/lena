using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.Risk;
using System;
using System.Linq;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public Risk AddRisk(
        string title,
        int? purchaseRequestId = null,
        int? purchaseOrderId = null,
        int? cargoItemId = null,
        int? customerComplaintId = null)
    {

      var risk = repository.Create<Risk>();
      risk.Title = title;
      risk.CreatorUserId = App.Providers.Security.CurrentLoginData.UserId;
      risk.CreateDateTime = DateTime.UtcNow;
      risk.PurchaseRequestId = purchaseRequestId;
      risk.PurchaseOrderId = purchaseOrderId;
      risk.CargoItemId = cargoItemId;
      repository.Add(risk);
      return risk;
    }
    #endregion

    #region Edit
    public Risk EditRisk(
        int id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<RiskStatus> riskStatus = null)
    {

      var risk = GetRisk(id: id);

      return EditRisk(
                    risk: risk,
                    rowVersion: rowVersion,
                    title: title,
                    riskStatus: riskStatus);
    }
    public Risk EditRisk(
        Risk risk,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<RiskStatus> riskStatus = null,
        TValue<RiskResolve> riskResolve = null)
    {

      if (title != null)
        risk.Title = title;
      if (riskStatus != null)
        risk.LatestRiskStatus = riskStatus;
      if (riskResolve != null)
        risk.LatestRiskResolve = riskResolve;

      repository.Update(entity: risk, rowVersion: rowVersion);
      return risk;
    }
    #endregion

    #region Delete
    public void DeleteRisk(Risk risk)
    {

      repository.Delete(risk);
    }
    #endregion

    #region DeleteProcess
    public void DeleteRiskProcess(int id)
    {

      var risk = GetRisk(id: id);

      if (risk.LatestRiskResolve != null)
      {
        throw new CannotDeleteRiskCasueHaveRiskResolveException(id: risk.Id);
      }

      if (risk.PurchaseRequestId.HasValue)
      {
        DeletePurchaseRequestRisk(purchaseRequestId: risk.PurchaseRequestId.Value, riskId: risk.Id);
      }

      if (risk.CargoItemId.HasValue)
      {
        DeleteCargoItemRisk(cargoItemId: risk.CargoItemId.Value, riskId: risk.Id);
      }

      if (risk.PurchaseOrderId.HasValue)
      {
        DeletePurchaseOrderRisk(purchaseOrderId: risk.PurchaseOrderId.Value, riskId: risk.Id);
      }
      EditRisk(risk: risk, rowVersion: risk.RowVersion, riskStatus: new TValue<RiskStatus>(null));
      DeleteRisk(risk: risk);
    }

    public void DeletePurchaseRequestRisk(int purchaseRequestId, int riskId)
    {

      var purchaseRequest = GetPurchaseRequest(id: purchaseRequestId);

      var purchaseRquestRisks = purchaseRequest.Risks.ToList();

      if (purchaseRquestRisks.Any(m => m.Id != riskId))
      {
        var latestRiskId = purchaseRquestRisks.Where(m => m.Id != riskId).Max(m => m.Id);

        var risk = purchaseRquestRisks.FirstOrDefault(m => m.Id == latestRiskId);

        EditPurchaseRequest(
               purchaseRequest: purchaseRequest,
               rowVersion: purchaseRequest.RowVersion,
               risk: risk);
      }
      else
      {

        EditPurchaseRequest(
                purchaseRequest: purchaseRequest,
                rowVersion: purchaseRequest.RowVersion,
                risk: new TValue<Risk>(null));
      }

    }

    public void DeletePurchaseOrderRisk(int purchaseOrderId, int riskId)
    {

      var purchaseOrder = GetPurchaseOrder(id: purchaseOrderId);

      var purchaseOrderRisks = purchaseOrder.Risks.ToList();

      if (purchaseOrderRisks.Any(m => m.Id != riskId))
      {
        var latestRiskId = purchaseOrderRisks.Where(m => m.Id != riskId).Max(m => m.Id);

        var risk = purchaseOrderRisks.FirstOrDefault(m => m.Id == latestRiskId);

        EditPurchaseOrder(
               purchaseOrder: purchaseOrder,
               rowVersion: purchaseOrder.RowVersion,
               risk: risk);
      }
      else
      {

        EditPurchaseOrder(
                purchaseOrder: purchaseOrder,
                rowVersion: purchaseOrder.RowVersion,
                risk: new TValue<Risk>(null));
      }

    }
    public void DeleteCargoItemRisk(int cargoItemId, int riskId)
    {

      var cargoItem = GetCargoItem(id: cargoItemId);

      var cargoItemRisks = cargoItem.Risks.ToList();

      if (cargoItemRisks.Any(m => m.Id != riskId))
      {
        var latestRiskId = cargoItemRisks.Where(m => m.Id != riskId).Max(m => m.Id);

        var risk = cargoItemRisks.FirstOrDefault(m => m.Id == latestRiskId);

        EditCargoItem(
               cargoItem: cargoItem,
               rowVersion: cargoItem.RowVersion,
               risk: risk);
      }
      else
      {

        EditCargoItem(
                cargoItem: cargoItem,
                rowVersion: cargoItem.RowVersion,
                risk: new TValue<Risk>(null));
      }

    }


    #endregion


    #region AddProcess
    public void AddPurchaseRequestRiskProcess(
        string title,
        OccurrenceSeverityStatus occurrenceSeverityStatus,
        OccurrenceProbabilityStatus occurrenceProbabilityStatus,
        int purchaseRequestId)
    {

      var purchaseRequest = GetPurchaseRequest(id: purchaseRequestId);

      var latestRisk = purchaseRequest.LatestRisk;
      if (latestRisk != null)
      {
        if (latestRisk.LatestRiskStatus.RiskParameter.RiskLevelStatus != RiskLevelStatus.Low)
        {
          throw new PurchaseRequestHasNotLowRiskLevelException(code: purchaseRequest.Code);
        }
      }


      var risk = AddRisk(title: title, purchaseRequestId: purchaseRequestId);

      var riskStatus = AddRiskStatus(
                riskId: risk.Id,
                occurrenceSeverityStatus: occurrenceSeverityStatus,
                occurrenceProbabilityStatus: occurrenceProbabilityStatus);

      EditRisk(
                risk: risk,
                rowVersion: risk.RowVersion,
                riskStatus: riskStatus);

      EditPurchaseRequest(
                purchaseRequest: purchaseRequest,
                rowVersion: purchaseRequest.RowVersion,
                risk: risk);

    }

    public void AddPurchaseOrderRiskProcess(
      string title,
      OccurrenceSeverityStatus occurrenceSeverityStatus,
      OccurrenceProbabilityStatus occurrenceProbabilityStatus,
      int purchaseOrderId)
    {

      var purchaseOrder = GetPurchaseOrder(id: purchaseOrderId);

      var latestRisk = purchaseOrder.LatestRisk;
      if (latestRisk != null)
      {
        if (latestRisk.LatestRiskStatus.RiskParameter.RiskLevelStatus != RiskLevelStatus.Low)
        {
          throw new PurchaseRequestHasNotLowRiskLevelException(code: purchaseOrder.Code);
        }
      }


      var risk = AddRisk(title: title, purchaseOrderId: purchaseOrderId);

      var riskStatus = AddRiskStatus(
                riskId: risk.Id,
                occurrenceSeverityStatus: occurrenceSeverityStatus,
                occurrenceProbabilityStatus: occurrenceProbabilityStatus);

      EditRisk(
                risk: risk,
                rowVersion: risk.RowVersion,
                riskStatus: riskStatus);

      EditPurchaseOrder(
                purchaseOrder: purchaseOrder,
                rowVersion: purchaseOrder.RowVersion,
                risk: risk);

    }


    public void AddCargoItemRiskProcess(
      string title,
      OccurrenceSeverityStatus occurrenceSeverityStatus,
      OccurrenceProbabilityStatus occurrenceProbabilityStatus,
      int cargoItemId)
    {

      var cargoItem = GetCargoItem(id: cargoItemId);

      var latestRisk = cargoItem.LatestRisk;
      if (latestRisk != null)
      {
        if (latestRisk.LatestRiskStatus.RiskParameter.RiskLevelStatus != RiskLevelStatus.Low)
        {
          throw new CargoItemHasNotLowRiskLevelException(code: cargoItem.Code);
        }
      }


      var risk = AddRisk(title: title, cargoItemId: cargoItemId);

      var riskStatus = AddRiskStatus(
                riskId: risk.Id,
                occurrenceSeverityStatus: occurrenceSeverityStatus,
                occurrenceProbabilityStatus: occurrenceProbabilityStatus);

      EditRisk(
                risk: risk,
                rowVersion: risk.RowVersion,
                riskStatus: riskStatus);

      EditCargoItem(
                cargoItem: cargoItem,
                rowVersion: cargoItem.RowVersion,
                risk: risk);

    }

    #endregion

    #region Get
    public Risk GetRisk(int id) => GetRisk(selector: e => e, id: id);
    public TResult GetRisk<TResult>(
        Expression<Func<Risk, TResult>> selector,
        int id)
    {

      var risk = GetRisks(selector: selector,
                id: id).FirstOrDefault();
      if (risk == null)
        throw new RiskNotFoundException(id: id);
      return risk;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetRisks<TResult>(
        Expression<Func<Risk, TResult>> selector,
        TValue<int> id = null,
        TValue<int> creatorUserId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<int> purchaseRequestId = null,
        TValue<int> purchaseOrderId = null,
        TValue<int> cargoItemId = null,
        TValue<RiskStatus> riskStatuses = null,
        TValue<int> customerComplaintId = null,
        TValue<RiskLevelStatus[]> hasStatus = null,
        TValue<RiskLevelStatus[]> notHasStatus = null)
    {


      var risk = repository.GetQuery<Risk>();

      if (id != null)
        risk = risk.Where(i => i.Id == id);
      if (creatorUserId != null)
        risk = risk.Where(i => i.CreatorUserId == creatorUserId);
      if (fromDateTime != null)
        risk = risk.Where(i => i.CreateDateTime >= fromDateTime);
      if (toDateTime != null)
        risk = risk.Where(i => i.CreateDateTime <= toDateTime);
      if (purchaseRequestId != null)
        risk = risk.Where(i => i.PurchaseRequestId == purchaseRequestId);
      if (purchaseOrderId != null)
        risk = risk.Where(i => i.PurchaseOrderId == purchaseOrderId);
      if (cargoItemId != null)
        risk = risk.Where(i => i.CargoItemId == cargoItemId);
      if (hasStatus != null)
        risk = risk.Where(i => hasStatus.Value.Contains(i.LatestRiskStatus.RiskParameter.RiskLevelStatus));
      if (notHasStatus != null)
        risk = risk.Where(i => !notHasStatus.Value.Contains(i.LatestRiskStatus.RiskParameter.RiskLevelStatus));
      if (riskStatuses != null)
        risk = risk.Where(i => i.RiskStatuses == riskStatuses);
      return risk.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<Risk, RiskResult>> ToRiskResult =
        risk =>
        new RiskResult
        {
          Id = risk.Id,
          Title = risk.Title,
          RiskLevelStatus = risk.LatestRiskStatus.RiskParameter.RiskLevelStatus,
          OccurrenceProbabilityStatus = risk.LatestRiskStatus.OccurrenceProbabilityStatus,
          OccurrenceSeverityStatus = risk.LatestRiskStatus.OccurrenceSeverityStatus,
          RiskFactor = (int)risk.LatestRiskStatus.OccurrenceProbabilityStatus * (int)risk.LatestRiskStatus.OccurrenceSeverityStatus,
          RiskCreatorUserId = risk.CreatorUserId,
          RiskCreatorEmployeeName = risk.CreatorUser.Employee.FirstName + " " + risk.CreatorUser.Employee.LastName,
          RiskCreatedDateTime = risk.CreateDateTime,
          CorrectiveAction = risk.LatestRiskResolve.CorrectiveAction,
          RiskResolveCreatorUserId = risk.LatestRiskResolve.CreatorUserId,
          RiskResolveCreatorEmployeeName = risk.LatestRiskResolve.CreatorUser.Employee.FirstName + " " + risk.LatestRiskResolve.CreatorUser.Employee.LastName,
          RiskResolveDateTime = risk.LatestRiskResolve.DateTime,
          RiskResolveStatus = risk.LatestRiskResolve.Status,
          RiskResolveId = risk.LatestRiskResolve.Id,
          ReviewerUserId = risk.LatestRiskResolve.ReviewerUserId,
          ReviewerEmployeeName = risk.LatestRiskResolve.ReviewerUser.Employee.FirstName + " " + risk.LatestRiskResolve.ReviewerUser.Employee.LastName,
          RevieweDateTime = risk.LatestRiskResolve.RevieweDateTime,
          ReviewDescription = risk.LatestRiskResolve.ReviewDescription,
          RiskRowVersion = risk.RowVersion,
          RiskResolveRowVersion = risk.LatestRiskResolve.RowVersion,
        };
    #endregion

    #region Search
    public IQueryable<RiskResult> SearchRiskResult(
        IQueryable<RiskResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(i =>
            i.Title.Contains(searchText) ||
            i.RiskCreatorEmployeeName.Contains(searchText) ||
            i.ReviewerEmployeeName.Contains(searchText) ||
            i.RiskResolveCreatorEmployeeName.Contains(searchText) ||
            i.ReviewDescription.Contains(searchText));

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<RiskResult> SortRiskResult(
        IQueryable<RiskResult> query,
        SortInput<RiskSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case RiskSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        case RiskSortType.Title:
          return query.OrderBy(i => i.Title, sortInput.SortOrder);
        case RiskSortType.RiskLevelStatus:
          return query.OrderBy(i => i.RiskLevelStatus, sortInput.SortOrder);
        case RiskSortType.RiskCreatorUserId:
          return query.OrderBy(i => i.RiskCreatorUserId, sortInput.SortOrder);
        case RiskSortType.RiskCreatorEmployeeName:
          return query.OrderBy(i => i.RiskCreatorEmployeeName, sortInput.SortOrder);
        case RiskSortType.RiskCreatedDateTime:
          return query.OrderBy(i => i.RiskCreatedDateTime, sortInput.SortOrder);
        case RiskSortType.CorrectiveAction:
          return query.OrderBy(i => i.CorrectiveAction, sortInput.SortOrder);
        case RiskSortType.RiskResolveCreatorUserId:
          return query.OrderBy(i => i.RiskResolveCreatorUserId, sortInput.SortOrder);
        case RiskSortType.RiskResolveCreatorEmployeeName:
          return query.OrderBy(i => i.RiskResolveCreatorEmployeeName, sortInput.SortOrder);
        case RiskSortType.RiskResolveDateTime:
          return query.OrderBy(i => i.RiskResolveDateTime, sortInput.SortOrder);
        case RiskSortType.RiskResolveStatus:
          return query.OrderBy(i => i.RiskResolveStatus, sortInput.SortOrder);
        case RiskSortType.ReviewerUserId:
          return query.OrderBy(i => i.ReviewerUserId, sortInput.SortOrder);
        case RiskSortType.ReviewerEmployeeName:
          return query.OrderBy(i => i.ReviewerEmployeeName, sortInput.SortOrder);
        case RiskSortType.RevieweDateTime:
          return query.OrderBy(i => i.RevieweDateTime, sortInput.SortOrder);
        case RiskSortType.ReviewDescription:
          return query.OrderBy(i => i.ReviewDescription, sortInput.SortOrder);
        case RiskSortType.RiskFactor:
          return query.OrderBy(i => i.RiskFactor, sortInput.SortOrder);
        case RiskSortType.OccurrenceProbabilityStatus:
          return query.OrderBy(i => i.OccurrenceProbabilityStatus, sortInput.SortOrder);
        case RiskSortType.OccurrenceSeverityStatus:
          return query.OrderBy(i => i.OccurrenceSeverityStatus, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

  }
}
