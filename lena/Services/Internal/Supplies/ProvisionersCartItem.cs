using System;
using System.Linq;
using System.Linq.Expressions;
//using System.Runtime.Remoting.Channels;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Core;
using lena.Services.Core.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.Common;
using lena.Models.Supplies.CargoItem;
using lena.Models.ApplicationBase.BaseEntityDocument;
using lena.Models.ApplicationBase.Unit;
using lena.Services.Internals.Accounting.Exception;
using System.Collections.Generic;
using lena.Models.StaticData;
using lena.Models.Supplies.ProvisionersCartItem;
using lena.Models.Supplies.PurchaseRequest;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add 
    public ProvisionersCartItem AddProvisionersCartItem(
        int? providerId,
        int provisionersCartId,
        PurchaseRequest purchaseRequest,
        double requestQty,
        double suppledQty
    )
    {
      var provisionersCartItem = repository.Create<ProvisionersCartItem>();
      provisionersCartItem.ProviderId = providerId;
      provisionersCartItem.RequestQty = requestQty;
      provisionersCartItem.SuppliedQty = suppledQty;
      provisionersCartItem.Status = ProvisionersCartItemStatus.NotCompleted;
      provisionersCartItem.ProvisionersCartId = provisionersCartId;
      provisionersCartItem.PurchaseRequest = purchaseRequest;
      repository.Add(provisionersCartItem);
      return provisionersCartItem;
    }
    #endregion
    #region Gets ProvisionersCartItem
    public IQueryable<TResult> GetProvisionersCartItems<TResult>(
       Expression<Func<ProvisionersCartItem, TResult>> selector,
       TValue<int> id = null,
       TValue<int> providerId = null,
       TValue<int> provisionersCartId = null,
       TValue<int> purchaseRequestId = null,
       TValue<double> requestQty = null,
       TValue<double> suppledQty = null
    )
    {
      var query = repository.GetQuery<ProvisionersCartItem>();
      if (id != null)
        query = query.Where(a => a.Id == id);
      if (providerId != null)
        query = query.Where(a => a.ProviderId == providerId);
      if (provisionersCartId != null)
        query = query.Where(a => a.ProvisionersCartId == provisionersCartId);
      if (purchaseRequestId != null)
        query = query.Where(a => a.PurchaseRequest.Id == purchaseRequestId);
      if (requestQty != null)
        query = query.Where(a => a.RequestQty == requestQty);
      if (suppledQty != null)
        query = query.Where(a => a.SuppliedQty == suppledQty);
      return query.Select(selector);
    }
    #endregion
    #region Get 
    public ProvisionersCartItem GetProvisionersCartItem(int id) => GetProvisionersCartItem(selector: e => e, id: id);
    public TResult GetProvisionersCartItem<TResult>(
        Expression<Func<ProvisionersCartItem, TResult>> selector,
        int id)
    {
      var provisionersPurchaseDetail = GetProvisionersCartItems(selector: selector, id: id)
            .FirstOrDefault();
      if (provisionersPurchaseDetail == null)
        throw new ProvisionersCartItemNotFoundException(id);
      return provisionersPurchaseDetail;
    }
    #endregion
    #region Edit
    public ProvisionersCartItem EditProvisionersCartItem(
        int id,
        byte[] rowVersion,
        TValue<int> providerId = null,
        TValue<double> requestQty = null,
        TValue<ProvisionersCartItemStatus> status = null,
        TValue<double> suppliedQty = null
        )
    {
      var provisionersCartItem = GetProvisionersCartItem(id: id);
      if (providerId != null)
        provisionersCartItem.ProviderId = providerId;
      if (requestQty != null)
        provisionersCartItem.RequestQty = requestQty;
      if (status != null)
        provisionersCartItem.Status = status;
      if (suppliedQty != null)
        provisionersCartItem.SuppliedQty = suppliedQty;
      repository.Update(rowVersion: rowVersion, entity: provisionersCartItem);
      return provisionersCartItem;
    }
    #endregion
    #region EditProcess
    public ProvisionersCartItem EditProvisionersCartItemProcess(
        int id,
        byte[] rowVersion,
        TValue<int> providerId = null,
        TValue<double> requestQty = null,
        TValue<ProvisionersCartItemStatus> status = null
        )
    {
      if (status == null)
      {
        var resetCartItemStatus = ResetProvisionersCartItemStatus(id);
      }
      var provisionersCartItem = EditProvisionersCartItem(
                        id: id,
                        rowVersion: rowVersion,
                        providerId: providerId,
                        requestQty: requestQty,
                        status: status
                       );
      return provisionersCartItem;
    }
    #endregion
    #region ToResult
    public Expression<Func<ProvisionersCartItem, ProvisionersCartItemResult>> ToProvisionersCartItemResult =
        (provisionersCartItem) => new ProvisionersCartItemResult
        {
          Id = provisionersCartItem.Id,
          ProviderId = provisionersCartItem.ProviderId,
          SuppliedQty = provisionersCartItem.SuppliedQty,
          RequestQty = provisionersCartItem.RequestQty,
          ProviderName = provisionersCartItem.Provider.Name,
          Status = provisionersCartItem.Status,
          RowVersion = provisionersCartItem.RowVersion,
          ProvisionersCartItemDetails = provisionersCartItem.ProvisionersCartItemDetails.AsQueryable().Select(App.Internals.Supplies.ToProvisionersCartItemDetailResult),
          PurchaseRequestId = provisionersCartItem.PurchaseRequest.Id,
          PurchaseRequestDeadline = provisionersCartItem.PurchaseRequest.Deadline,
          PurchaseRequestStuffCode = provisionersCartItem.PurchaseRequest.Stuff.Code,
          PurchaseRequestStuffName = provisionersCartItem.PurchaseRequest.Stuff.Name,
          PurchaseRequestUnitId = provisionersCartItem.PurchaseRequest.UnitId,
          PurchaseRequestStuffId = provisionersCartItem.PurchaseRequest.Stuff.Id,
          PurchaseRequestUnitName = provisionersCartItem.PurchaseRequest.Unit.Name,
          PurchaseRequestOrderedQty = provisionersCartItem.PurchaseRequest.PurchaseRequestSummary.OrderedQty,
          PurchaseRequestQty = provisionersCartItem.PurchaseRequest.Qty
        };
    #endregion
    #region ResetStatus
    public ProvisionersCartItem ResetProvisionersCartItemStatus(int provisionersCartItemId)
    {
      var provisionersCartItem = GetProvisionersCartItem(id: provisionersCartItemId);
      return ResetProvisionersCartItemStatus(provisionersCartItem: provisionersCartItem);
    }
    public ProvisionersCartItem ResetProvisionersCartItemStatus(ProvisionersCartItem provisionersCartItem)
    {
      var provisionersCartItemDetails = GetProvisionersCartItemDetails(
                    selector: e => e,
                    provisionersCartItemId: provisionersCartItem.Id);
      var suppliedQty = provisionersCartItemDetails.Sum(x => x.SupplyQty == null ? 0 : x.SupplyQty);
      if (suppliedQty == provisionersCartItem.RequestQty)
      {
        provisionersCartItem.Status = ProvisionersCartItemStatus.Completed;
      }
      else
      {
        provisionersCartItem.Status = ProvisionersCartItemStatus.NotCompleted;
      }
      var editProvisionersCartItem = EditProvisionersCartItem(id: provisionersCartItem.Id, rowVersion: provisionersCartItem.RowVersion, status: provisionersCartItem.Status);
      //repository.Update(rowVersion: provisionersCartItem.RowVersion, entity: editProvisionersCartItem);
      var resetCartStatus = ResetProvisionersCartStatus(provisionersCartItem.ProvisionersCartId);
      return editProvisionersCartItem;
    }
    #endregion
    #region DeleteProcess
    public void DeleteProvisionersCartItemProcess(int id)
    {
      var provisionersCartItem = GetProvisionersCartItem(id: id);
      if (provisionersCartItem.SuppliedQty > 0)
      {
        throw new CantDeleteProvisionersCartItemException(provisionersCartItem.SuppliedQty);
      }
      DeleteProvisionersCartItem(provisionersCartItem.Id);
    }
    #endregion
    #region Delete
    public void DeleteProvisionersCartItem(int id)
    {
      var provisionersCartItem = GetProvisionersCartItem(id: id);
      repository.Delete(provisionersCartItem);
    }
    #endregion
  }
}