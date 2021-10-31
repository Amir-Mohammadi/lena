using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.GeneralStuffRequest;
using System;
using System.Linq;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add

    public GeneralStuffRequestDetail AddGeneralStuffRequestDetail(
        GeneralStuffRequestDetail requestDetail,
        int generalStuffRequestId,
        int? stuffRequestId,
        int? purchaseRequestId,
        int? alternativePurchaseRequestId,
        double qty,
        byte unitId,
        string description
        )
    {

      requestDetail = requestDetail ?? repository.Create<GeneralStuffRequestDetail>();
      requestDetail.GeneralStuffRequestId = generalStuffRequestId;
      requestDetail.StuffRequestId = stuffRequestId;
      requestDetail.PurchaseRequestId = purchaseRequestId;
      requestDetail.AlternativePurchaseRequestId = alternativePurchaseRequestId;
      requestDetail.UnitId = unitId;
      requestDetail.Qty = qty;
      requestDetail.Description = description;

      repository.Add(requestDetail);

      return requestDetail;
    }

    #endregion

    #region AddProcess
    public GeneralStuffRequestDetail AddGeneralStuffRequestDetailProcess(
        GeneralStuffRequestDetail requestDetail,
        int generalStuffRequestId,
        int? stuffRequestId,
        int? purchaseRequestId,
        int? alternativePurchaseRequestId,
        double qty,
        byte unitId,
        string description)
    {


      #region AddGeneralStuffRequestDetail

      requestDetail = AddGeneralStuffRequestDetail(
              requestDetail: requestDetail,
              generalStuffRequestId: generalStuffRequestId,
              stuffRequestId: stuffRequestId,
              purchaseRequestId: purchaseRequestId,
              alternativePurchaseRequestId: alternativePurchaseRequestId,
              qty: qty,
              unitId: unitId,
              description: description);

      #endregion
      return requestDetail;
    }

    #endregion

    #region Edit

    public GeneralStuffRequestDetail EditGeneralStuffRequestDetail(
        int id,
        byte[] rowVersion,
        TValue<int> generalStuffRequestId = null,
        TValue<int?> stuffRequestId = null,
        TValue<int?> purchaseRequestId = null,
        TValue<int?> alternativePurchaseRequestId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<string> description = null)
    {

      var generalStuffRequestDetail = GetGeneralStuffRequestDetail(id: id);
      return EditGeneralStuffRequestDetail(
                    requestDetail: generalStuffRequestDetail,
                    rowVersion: rowVersion,
                    generalStuffRequestId: generalStuffRequestId,
                    stuffRequestId: stuffRequestId,
                    purchaseRequestId: purchaseRequestId,
                    alternativePurchaseRequestId: alternativePurchaseRequestId,
                    qty: qty,
                    unitId: unitId,
                    description: description);
    }

    public GeneralStuffRequestDetail EditGeneralStuffRequestDetail(
        GeneralStuffRequestDetail requestDetail,
        byte[] rowVersion,
        TValue<int> generalStuffRequestId = null,
        TValue<int?> stuffRequestId = null,
        TValue<int?> purchaseRequestId = null,
        TValue<int?> alternativePurchaseRequestId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<string> description = null)
    {

      if (generalStuffRequestId != null)
        requestDetail.GeneralStuffRequestId = generalStuffRequestId;
      if (stuffRequestId != null)
        requestDetail.StuffRequestId = stuffRequestId;
      if (purchaseRequestId != null)
        requestDetail.PurchaseRequestId = purchaseRequestId;
      if (alternativePurchaseRequestId != null)
        requestDetail.AlternativePurchaseRequestId = alternativePurchaseRequestId;

      if (unitId != null)
        requestDetail.UnitId = unitId;
      if (qty != null)
        requestDetail.Qty = qty;
      if (description != null)
        requestDetail.Description = description;

      repository.Update(requestDetail, rowVersion);

      return requestDetail;
    }

    #endregion

    #region RemoveProcess

    public void DeleteGeneralStuffRequestDetail(int id)
    {

      var generalStuffRequestDetail = GetGeneralStuffRequestDetail(id: id);

      repository.Delete(generalStuffRequestDetail);

    }

    #endregion

    #region Get

    public GeneralStuffRequestDetail GetGeneralStuffRequestDetail(int id) => GetGeneralStuffRequestDetail(selector: e => e, id: id);

    public TResult GetGeneralStuffRequestDetail<TResult>(
        Expression<Func<GeneralStuffRequestDetail, TResult>> selector,
        int id)
    {

      var requestDetail = GetGeneralStuffRequestDetails(
                    selector: selector,
                    id: id)


                .FirstOrDefault();

      if (requestDetail == null)
        throw new GeneralStuffRequestDetailNotFoundException(id);

      return requestDetail;
    }

    public IQueryable<TResult> GetGeneralStuffRequestDetails<TResult>(
        Expression<Func<GeneralStuffRequestDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<int> generalStuffRequestId = null,
        TValue<int?> stuffRequestId = null,
        TValue<int?> purchaseRequestId = null,
        TValue<int?> alternativePurchaseRequestId = null,
        TValue<int> stuffId = null,
        TValue<double> qty = null)
    {

      var query = repository.GetQuery<GeneralStuffRequestDetail>();

      if (id != null)
        query = query.Where(r => r.Id == id);
      if (stuffId != null)
        query = query.Where(r => r.GeneralStuffRequest.StuffId == stuffId);
      if (generalStuffRequestId != null)
        query = query.Where(r => r.GeneralStuffRequestId == generalStuffRequestId);
      if (stuffRequestId != null)
        query = query.Where(r => r.StuffRequestId == stuffRequestId);
      if (purchaseRequestId != null)
        query = query.Where(r => r.PurchaseRequestId == purchaseRequestId);
      if (purchaseRequestId != null)
        query = query.Where(r => r.AlternativePurchaseRequestId == alternativePurchaseRequestId);
      if (qty != null)
        query = query.Where(r => r.Qty == qty);

      return query.Select(selector);
    }

    #endregion

    #region Search

    public IQueryable<GeneralStuffRequestDetailResult> SearchGeneralStuffRequestDetailResult(IQueryable<GeneralStuffRequestDetailResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = query.Where(t => t.UnitName.Contains(search) || t.Description.Contains(search));

      return query;
    }

    #endregion



    #region Sort

    public IOrderedQueryable<GeneralStuffRequestDetailResult> SortGeneralStuffRequestDetailResult(IQueryable<GeneralStuffRequestDetailResult> query,
        SortInput<GeneralStuffRequestDetailSortType> sort)
    {
      switch (sort.SortType)
      {
        case GeneralStuffRequestDetailSortType.Id:
          return query.OrderBy(t => t.Id, sort.SortOrder);
        case GeneralStuffRequestDetailSortType.GeneralStuffRequestId:
          return query.OrderBy(t => t.GeneralStuffRequestId, sort.SortOrder);
        case GeneralStuffRequestDetailSortType.Qty:
          return query.OrderBy(t => t.Qty, sort.SortOrder);
        case GeneralStuffRequestDetailSortType.UnitName:
          return query.OrderBy(t => t.UnitName, sort.SortOrder);
        case GeneralStuffRequestDetailSortType.Description:
          return query.OrderBy(t => t.Description, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    #endregion


    #region ToGeneralStuffRequestDetailResult
    public Expression<Func<GeneralStuffRequestDetail, GeneralStuffRequestDetailResult>> ToGeneralStuffRequestDetailResult =
        requestDetail => new GeneralStuffRequestDetailResult
        {
          Id = requestDetail.Id,
          GeneralStuffRequestId = requestDetail.GeneralStuffRequestId,
          PurchaseRequestId = requestDetail.PurchaseRequestId,
          PurchaseRequestCode = requestDetail.PurchaseRequest.Code,
          AlternativePurchaseRequestId = requestDetail.AlternativePurchaseRequestId,
          AlternativePurchaseRequestCode = requestDetail.AlternativePurchaseRequest.Code,
          StuffRequestId = requestDetail.StuffRequestId,
          StuffRequestCode = requestDetail.StuffRequest.Code,
          Qty = requestDetail.Qty,
          UnitId = requestDetail.UnitId,
          UnitName = requestDetail.Unit.Name,
          Description = requestDetail.Description,
          RowVersion = requestDetail.RowVersion
        };

    #endregion




  }
}