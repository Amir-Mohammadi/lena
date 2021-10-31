using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Supplies.ProvisionersCartItemDetail;
using lena.Models.Supplies.PurchaseOrder;
using lena.Models.Supplies.PurchaseOrderGroup;
using System;
using System.Collections.Generic;
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
    public ProvisionersCartItemDetail AddProvisionersCartItemDetail(
        int providerId,
        int provisionersCartItemId,
        double supplyQty,
        int unitPrice,
        int currencyId,
        PurchaseOrder purchaseOrder,
        string description
    )
    {
      var provisionersCartItemDetail = repository.Create<ProvisionersCartItemDetail>();
      provisionersCartItemDetail.ProviderId = providerId;
      provisionersCartItemDetail.ProvisionersCartItemId = provisionersCartItemId;
      provisionersCartItemDetail.SupplyQty = supplyQty;
      provisionersCartItemDetail.DateTime = DateTime.UtcNow;
      provisionersCartItemDetail.UnitPrice = unitPrice;
      provisionersCartItemDetail.Description = description;
      provisionersCartItemDetail.CurrencyId = currencyId;
      provisionersCartItemDetail.PurchaseOrder = purchaseOrder;
      repository.Add(provisionersCartItemDetail);
      return provisionersCartItemDetail;
    }
    #endregion
    #region AddProcess
    public ProvisionersCartItemDetail AddProvisionersCartItemDetailProcess(
        int providerId,
        int provisionersCartItemId,
        double supplyQty,
        int unitPrice,
        byte currencyId,
        string description,
        DateTime purchaseOrderPreparingDateTime,
        int? supplierId,
        int purchaseRequestId,
        int stuffId,
        byte unitId,
        DateTime deadline,
        double? price,
        double qty,
        PurchaseOrderGroupItemInput[] purchaseOrderGroupItems
        )
    {
      var suppliesModule = App.Internals.Supplies;
      var purchaseOrderGroup = suppliesModule.AddPurchaseOrderGroupProcess(
                transactionBatch: null,
                financialTransactionBatch: null,
                description: description,
                purchaseOrderGroupItems: purchaseOrderGroupItems
                );
      var purchaseOrderInputs = new List<AddPurchaseOrderInput>();
      AddPurchaseOrderInput addPurchaseOrderInput = new AddPurchaseOrderInput();
      addPurchaseOrderInput.CurrencyId = currencyId;
      addPurchaseOrderInput.Deadline = deadline;
      addPurchaseOrderInput.OrderInvoiceNum = null;
      addPurchaseOrderInput.ProviderId = providerId;
      addPurchaseOrderInput.PurchaseOrderGroupId = purchaseOrderGroup.Id;
      addPurchaseOrderInput.PurchaseOrderPreparingDateTime = purchaseOrderPreparingDateTime;
      addPurchaseOrderInput.Qty = qty;
      addPurchaseOrderInput.UnitId = unitId;
      addPurchaseOrderInput.StuffId = stuffId;
      addPurchaseOrderInput.PurchaseOrderDateTime = DateTime.UtcNow;
      addPurchaseOrderInput.PurchaseRequestId = purchaseRequestId;
      addPurchaseOrderInput.Price = price;
      purchaseOrderInputs.Add(addPurchaseOrderInput);
      var purchaseOrders = suppliesModule.AddPurchaseOrdersProcess(
                financialDocumentCost: null,
                financialDocumentDiscount: null,
                document: null,
                addPurchaseOrders: purchaseOrderInputs.ToArray(),
                purchaseOrderType: PurchaseOrderType.SupplierOrder
                ); ; var getProvisionersCartItem = GetProvisionersCartItem(id: provisionersCartItemId);
      getProvisionersCartItem.SuppliedQty += supplyQty;
      var provisionersCartItem = EditProvisionersCartItem(id: provisionersCartItemId, rowVersion: getProvisionersCartItem.RowVersion, suppliedQty: getProvisionersCartItem.SuppliedQty);
      ProvisionersCartItemDetail provisionersCartItemDetail = null;
      foreach (var purchaseOrder in purchaseOrders)
      {
        provisionersCartItemDetail = AddProvisionersCartItemDetail(
                  providerId: providerId,
                  provisionersCartItemId: provisionersCartItemId,
                  supplyQty: supplyQty,
                  unitPrice: unitPrice,
                  currencyId: currencyId,
                  purchaseOrder: purchaseOrder,
                  description: description
                 );
      }
      var resetCartItemStatus = ResetProvisionersCartItemStatus(provisionersCartItemId);
      return provisionersCartItemDetail;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProvisionersCartItemDetails<TResult>(
       Expression<Func<ProvisionersCartItemDetail, TResult>> selector,
       TValue<int> id = null,
       TValue<int> providerId = null,
       TValue<int> provisionersCartItemId = null,
       TValue<int> provisionerCartId = null,
       TValue<double> supplyQty = null
    )
    {
      var query = repository.GetQuery<ProvisionersCartItemDetail>();
      if (id != null)
        query = query.Where(a => a.Id == id);
      if (providerId != null)
        query = query.Where(a => a.ProviderId == providerId);
      if (provisionersCartItemId != null)
        query = query.Where(a => a.ProvisionersCartItemId == provisionersCartItemId);
      if (provisionerCartId != null)
        query = query.Where(a => a.ProvisionersCartItem.ProvisionersCartId == provisionerCartId);
      if (supplyQty != null)
        query = query.Where(a => a.SupplyQty == supplyQty);
      return query.Select(selector);
    }
    #endregion
    #region Get 
    public ProvisionersCartItemDetail GetProvisionersCartItemDetail(int id) => GetProvisionersCartItemDetail(selector: e => e, id: id);
    public TResult GetProvisionersCartItemDetail<TResult>(
        Expression<Func<ProvisionersCartItemDetail, TResult>> selector,
        int id)
    {
      var provisionersPurchaseDetail = GetProvisionersCartItemDetails(selector: selector, id: id)
            .FirstOrDefault();
      if (provisionersPurchaseDetail == null)
        throw new ProvisionersCartItemNotFoundException(id);
      return provisionersPurchaseDetail;
    }
    #endregion
    #region Delete
    public void DeleteProvisionersCartItemDetail(int id)
    {
      var provisionersCartItemDetail = GetProvisionersCartItemDetail(id: id);
      repository.Delete(provisionersCartItemDetail);
    }
    #endregion
    #region DeleteProcess
    public void DeleteProvisionersCartItemDetailProcess(int id)
    {
      var provisionersCartItemDetail = GetProvisionersCartItemDetail(id: id);
      if (provisionersCartItemDetail.PurchaseOrder != null)
      {
        var purchaseOrder = App.Internals.Supplies.GetPurchaseOrder(provisionersCartItemDetail.PurchaseOrder.Id);
        if (purchaseOrder.IsDelete == false)
          throw new CantDeleteProvisionersCartItemDetailException(purchaseOrder.Code);
      }
      var getProvisionersCartItem = GetProvisionersCartItem(id: provisionersCartItemDetail.ProvisionersCartItemId);
      getProvisionersCartItem.SuppliedQty -= provisionersCartItemDetail.SupplyQty;
      if (getProvisionersCartItem.SuppliedQty == getProvisionersCartItem.RequestQty)
      {
        getProvisionersCartItem.Status = ProvisionersCartItemStatus.Completed;
      }
      else
      {
        getProvisionersCartItem.Status = ProvisionersCartItemStatus.NotCompleted;
      }
      var provisionersCartItem = EditProvisionersCartItem(id: provisionersCartItemDetail.ProvisionersCartItemId, rowVersion: getProvisionersCartItem.RowVersion, suppliedQty: getProvisionersCartItem.SuppliedQty, status: getProvisionersCartItem.Status);
      var resetCartStatus = ResetProvisionersCartStatus(provisionersCartItem.ProvisionersCartId);
      var resetCartItemStatus = ResetProvisionersCartItemStatus(provisionersCartItem.Id);
      DeleteProvisionersCartItemDetail(provisionersCartItemDetail.Id);
    }
    #endregion
    #region Edit
    public ProvisionersCartItemDetail EditProvisionersCartItemDetail(
        int id,
        byte[] rowVersion,
        TValue<int> unitPrice = null,
        TValue<int> currencyId = null,
        TValue<int> providerId = null,
        TValue<double> supplyQty = null
        )
    {
      var provisionersCartItemDetail = GetProvisionersCartItemDetail(id: id);
      if (providerId != null)
        provisionersCartItemDetail.ProviderId = providerId;
      if (supplyQty != null)
        provisionersCartItemDetail.SupplyQty = supplyQty;
      if (unitPrice != null)
        provisionersCartItemDetail.UnitPrice = unitPrice;
      if (currencyId != null)
        provisionersCartItemDetail.CurrencyId = currencyId;
      provisionersCartItemDetail.DateTime = DateTime.UtcNow;
      repository.Update(rowVersion: rowVersion, entity: provisionersCartItemDetail);
      return provisionersCartItemDetail;
    }
    #endregion
    #region EditProcess
    public ProvisionersCartItemDetail EditProvisionersCartItemDetailProcess(
        int id,
        byte[] rowVersion,
        TValue<int> unitPrice = null,
        TValue<int> currencyId = null,
        TValue<int> providerId = null,
        TValue<double> supplyQty = null,
        TValue<int> provisionersCartItemId = null
        )
    {
      var provisionersCartItem = GetProvisionersCartItem(id: provisionersCartItemId); ; var getProvisionersCartItemDetail = GetProvisionersCartItemDetail(id: id);
      provisionersCartItem.SuppliedQty += (supplyQty - getProvisionersCartItemDetail.SupplyQty);
      if (provisionersCartItem.SuppliedQty == provisionersCartItem.RequestQty)
      {
        provisionersCartItem.Status = ProvisionersCartItemStatus.Completed;
      }
      else
      {
        provisionersCartItem.Status = ProvisionersCartItemStatus.NotCompleted;
      }
      var provisionersCartItems = EditProvisionersCartItem(id: provisionersCartItemId, rowVersion: provisionersCartItem.RowVersion, suppliedQty: provisionersCartItem.SuppliedQty, status: provisionersCartItem.Status); ; var resetCartStatus = ResetProvisionersCartStatus(provisionersCartItem.ProvisionersCartId);
      var provisionersCartItemDetail = EditProvisionersCartItemDetail(
                id: id,
                providerId: providerId,
                rowVersion: rowVersion,
                supplyQty: supplyQty,
                unitPrice: unitPrice,
                currencyId: currencyId
               );
      return provisionersCartItemDetail;
    }
    #endregion
    #region ToResult
    public Expression<Func<ProvisionersCartItemDetail, ProvisionersCartItemDetailResult>> ToProvisionersCartItemDetailResult =
        (provisionersCartItemDetail) => new ProvisionersCartItemDetailResult
        {
          Id = provisionersCartItemDetail.Id,
          RowVersion = provisionersCartItemDetail.RowVersion,
          ProviderId = provisionersCartItemDetail.ProviderId,
          ProviderName = provisionersCartItemDetail.Provider.Name,
          SupplyQty = provisionersCartItemDetail.SupplyQty,
          DateTime = provisionersCartItemDetail.DateTime,
          UnitPrice = provisionersCartItemDetail.UnitPrice,
          CurrencyId = provisionersCartItemDetail.CurrencyId,
          Description = provisionersCartItemDetail.Description,
          CurrencyTitle = provisionersCartItemDetail.PurchaseOrder.Currency.Title,
          PurchaseOrderCode = provisionersCartItemDetail.PurchaseOrder.IsDelete == false ? provisionersCartItemDetail.PurchaseOrder.Code : null
        };
    #endregion
  }
}