using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Common;
using lena.Services.Common.Utilities;
using lena.Services.Core;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Services.Internals.QualityControl.Exception;
using lena.Services.Internals.Supplies.Exception;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Accounting.FinancialAccount;
using lena.Models.Accounting.PayRequest;
using lena.Models.Common;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Get
    public PayRequest GetPayRequest(int id) => GetPayRequest(selector: e => e, id: id);
    public TResult GetPayRequest<TResult>(
        Expression<Func<PayRequest, TResult>> selector,
        int id)
    {
      var payRequest = GetPayRequests(
                    selector: selector,
                    id: id)


                .SingleOrDefault();
      if (payRequest == null)
        throw new PayRequestNotFoundException(id);
      return payRequest;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetPayRequests<TResult>(
        Expression<Func<PayRequest, TResult>> selector,
        TValue<int[]> ids = null,
        TValue<int> id = null,
        TValue<string> description = null,
        TValue<bool> isDelete = null,
        TValue<int> employeeId = null,
        TValue<double> payedAmount = null,
        TValue<PayRequestStatus> status = null,
        TValue<string> qualityControlCode = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null)
    {
      var query = repository.GetQuery<PayRequest>();
      if (ids != null && ids.Value.Length != 0)
        query = query.Where(x => ids.Value.Contains(x.Id));
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (qualityControlCode != null)
        query = query.Where(x => x.QualityControl.Code == qualityControlCode);
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (payedAmount != null)
        query = query.Where(i => i.PayedAmount == payedAmount);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (isDelete != null)
        query = query.Where(i => i.IsDelete == isDelete);
      if (employeeId != null)
        query = query.Where(i => i.User.Employee.Id == employeeId);
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public PayRequest AddPayRequestProcess(
        int qualityControlId,
        double? discountedTotalPrice,
        string description)
    {
      var qualityControl = App.Internals.QualityControl.GetQualityControl(
                    id: qualityControlId);
      if (qualityControl == null)
        throw new QualityControlNotFoundException(id: qualityControlId);
      if (qualityControl.PayRequest != null)
        throw new QualityControlHasPayRequestException(qualityControlCode: qualityControl.Code);
      return AddPayRequest(
                    qualityControl: qualityControl,
                    financialTransactionBatch: null,
                    payedAmount: 0,
                    discountedTotalPrice: discountedTotalPrice,
                    documentId: null,
                    description: description);
    }
    public PayRequest AddPayRequest(
        lena.Domains.QualityControl qualityControl,
        FinancialTransactionBatch financialTransactionBatch,
        Guid? documentId,
        double payedAmount,
        double? discountedTotalPrice,
        string description)
    {
      var payRequest = repository.Create<PayRequest>();
      payRequest.AddDescription(description);
      payRequest.AddRemovable();
      payRequest.AddSaveLog();
      payRequest.QualityControl = qualityControl;
      payRequest.FinancialTransactionBatch = financialTransactionBatch;
      payRequest.PayedAmount = payedAmount;
      payRequest.DocumentId = documentId;
      payRequest.Description = description;
      payRequest.Status = PayRequestStatus.NotAction;
      payRequest.DiscountedTotalPrice = discountedTotalPrice;
      repository.Add(payRequest);
      return payRequest;
    }
    #endregion
    #region EditPayRequest
    public PayRequest EditPayRequest(
        PayRequest payRequest,
        byte[] rowVersion,
        TValue<lena.Domains.QualityControl> qualityControl = null,
        TValue<string> description = null,
        TValue<int> userId = null,
        TValue<int> financialTransactionBatchId = null,
        TValue<double> payedAmount = null,
        TValue<PayRequestStatus> status = null,
        TValue<DateTime> dateTime = null,
        TValue<Guid?> documentId = null,
        TValue<bool> isDelete = null)
    {
      if (qualityControl != null)
        payRequest.QualityControl = qualityControl;
      if (userId != null)
        payRequest.UserId = userId;
      if (financialTransactionBatchId != null)
        payRequest.FinancialTransactionBatchId = financialTransactionBatchId;
      if (payedAmount != null)
        payRequest.PayedAmount = payedAmount;
      if (dateTime != null)
        payRequest.DateTime = dateTime;
      if (status != null)
        payRequest.Status = status;
      if (description != null)
        payRequest.Description = description;
      if (documentId != null)
        payRequest.DocumentId = documentId;
      if (isDelete != null)
        payRequest.IsDelete = isDelete;
      repository.Update(rowVersion: rowVersion, entity: payRequest);
      return payRequest;
    }
    #endregion
    #region Reject
    public PayRequest RejectPayRequestProcess(
        int id,
        byte[] rowVersion)
    {
      var payReqeust = GetPayRequest(id: id);
      if (payReqeust.Status == PayRequestStatus.Accepted)
        throw new PayRequestIsAcceptedException(id: id);
      return EditPayRequest(
                    payRequest: payReqeust,
                    rowVersion: rowVersion,
                    status: PayRequestStatus.Rejected);
    }
    #endregion
    #region Add
    public PayRequest AcceptPayRequestProcess(
        int id,
        double amount,
        UploadFileData uploadFileData,
        byte[] rowVersion)
    {
      if (uploadFileData == null) throw new DocumentIsNullException();
      var document = App.Internals.ApplicationBase.AddDocument(
                    name: uploadFileData.FileName,
                    fileStream: uploadFileData.FileData);
      var payRequest = GetPayRequest(id: id);
      if (payRequest == null)
        throw new PayRequestNotFoundException(id);
      if (payRequest.Status == PayRequestStatus.Accepted)
        throw new PayRequestIsAcceptedException(id: id);
      #region Check Amount
      var payRequestsQuery = GetPayRequests(
              selector: e => e,
              id: id,
              isDelete: false); ; var payRequestResults = ToPayRequestResult(query: payRequestsQuery);
      var payRequestResult = payRequestResults.FirstOrDefault();
      if (payRequestResult == null)
        throw new PayRequestNotFoundException(id);
      if (amount < 0)
        throw new FinancialTransactionAmountIsMoreThanStuffTotalPriceException(payRequestId: id);
      #endregion
      #region Check ReceiptStatus
      if (payRequestResult.ReceiptStatus.HasValue && payRequestResult.ReceiptStatus.Value.HasFlag(ReceiptStatus.EternalReceipt))
        throw new ReceiptIsInEternalReceiptStatusException(id: payRequestResult.ReceiptId.Value);
      #endregion
      #region FinancialTransaction
      FinancialTransactionBatch financialTransactionBatch = null;
      if (amount > 0)
      {
        if (payRequestResult.ProviderId == null)
          throw new PurchaseOrderHasNoProviderException(
                    purchaseOrderId: payRequestResult.PurchaseOrderId ?? -1,
                    purchaseOrderCode: payRequestResult.PurchaseOrderCode);
        var cooperatorFinancialAccount = App.Internals.Accounting.GetCooperatorFinancialAccounts(
                      selector: e => e,
                      cooperatorId: payRequestResult.ProviderId,
                      currencyId: payRequestResult.CurrencyId)


                  .FirstOrDefault();
        if (cooperatorFinancialAccount == null)
          throw new CooperatorHasNoFinancialAccountException(cooperatorId: payRequestResult.ProviderId.Value);
        financialTransactionBatch = App.Internals.Accounting.AddFinancialTransactionBatch();
        App.Internals.Accounting.AddFinancialTransactionProcess(
                      financialTransaction: null,
                      amount: amount,
                      effectDateTime: DateTime.Now.ToUniversalTime(),
                      description: null,
                      financialAccountId: cooperatorFinancialAccount.Id,
                      financialTransactionType: Models.StaticData.StaticFinancialTransactionTypes.QualityControlRejected,
                      financialTransactionBatchId: financialTransactionBatch.Id,
                      referenceFinancialTransaction: null);
      }
      #endregion
      return EditPayRequest(
              payRequest: payRequest,
              rowVersion: rowVersion,
              payedAmount: amount,
              financialTransactionBatchId: financialTransactionBatch?.Id,
              documentId: document.Id,
              status: PayRequestStatus.Accepted);
    }
    #endregion
    #region ToResult
    public IQueryable<PayRequestResult> ToPayRequestResult(
     IQueryable<PayRequest> query)
    {
      var receiptQualityControls =
                App.Internals.QualityControl.GetReceiptQualityControls(selector: e => new
                {
                  Id = e.Id,
                  StoreReceiptId = e.StoreReceiptId,
                  StoreReceiptCode = e.StoreReceipt.Code,
                  ReceiptId = e.StoreReceipt.Receipt.Id,
                  ReceiptStatus = e.StoreReceipt.Receipt.Status,
                  StuffId = e.StuffId,
                  StuffCode = e.Stuff.Code,
                  StuffName = e.Stuff.Name,
                });
      var newShoppingsQuery = App.Internals.WarehouseManagement.GetNewShoppings(selector: e =>
                    new
                    {
                      Id = e.Id,
                      CargoId = e.LadingItem.CargoItem.CargoId,
                      CargoCode = e.LadingItem.CargoItem.Cargo.Code,
                      CargoItemCode = e.LadingItem.CargoItem.Code,
                      CargoItemId = e.LadingItem.CargoItemId,
                      LadingId = e.LadingItem.LadingId,
                      LadingCode = e.LadingItem.Lading.Code,
                      PurchaseOrderCode = e.LadingItem.CargoItem.PurchaseOrder.Code,
                      PurchaseOrderId = e.LadingItem.CargoItem.PurchaseOrderId,
                      UnitPrice = e.LadingItem.CargoItem.PurchaseOrder.Price,
                      CurrencyId = e.LadingItem.CargoItem.PurchaseOrder.CurrencyId,
                      CurrencyTitle = e.LadingItem.CargoItem.PurchaseOrder.Currency.Title,
                      ProviderId = e.LadingItem.CargoItem.PurchaseOrder.ProviderId,
                      ProviderName = e.LadingItem.CargoItem.PurchaseOrder.Provider.Name,
                      FinancialAccountCode =
                            e.LadingItem.CargoItem.PurchaseOrder.Provider.CooperatorFinancialAccount.FirstOrDefault(
                                i => i.CurrencyId == e.LadingItem.CargoItem.PurchaseOrder.CurrencyId).Code
                    });
      var result = from payRequest in query
                   join receiptQualityControl in receiptQualityControls on payRequest.QualityControl.Id equals
                             receiptQualityControl.Id into tblReceiptQualityControl
                   from subRqc in tblReceiptQualityControl.DefaultIfEmpty()
                   join newShopping in newShoppingsQuery on subRqc.StoreReceiptId equals newShopping.Id into
                             tblNewShopping
                   from subNewShopping in tblNewShopping.DefaultIfEmpty()
                   select new PayRequestResult
                   {
                     Id = payRequest.Id,
                     QualityControlCode = payRequest.QualityControl.Code,
                     QualityControlId = payRequest.QualityControl.Id,
                     Description = payRequest.Description,
                     PayedAmount = payRequest.PayedAmount,
                     DiscountedTotalPrice = payRequest.DiscountedTotalPrice,
                     DocumentId = payRequest.DocumentId,
                     QualityControlDocumentId = payRequest.QualityControl.DocumentId,
                     FinancialTransactionBatchId = payRequest.FinancialTransactionBatchId,
                     Status = payRequest.Status,
                     EmployeeFullName = payRequest.User.Employee.FirstName + " " + payRequest.User.Employee.LastName,
                     DateTime = payRequest.DateTime,
                     ReceiptId = subRqc.ReceiptId,
                     ReceiptStatus = subRqc.ReceiptStatus,
                     StuffId = subRqc.StuffId,
                     StuffCode = subRqc.StuffCode,
                     StuffName = subRqc.StuffName,
                     StoreReceiptId = subRqc.StoreReceiptId,
                     StoreReceiptCode = subRqc.StoreReceiptCode,
                     ProviderId = subNewShopping.ProviderId,
                     ProviderName = subNewShopping.ProviderName,
                     CargoId = subNewShopping.CargoId,
                     CargoCode = subNewShopping.CargoCode,
                     CargoItemId = subNewShopping.CargoItemId,
                     CargoItemCode = subNewShopping.CargoItemCode,
                     LadingCode = subNewShopping.LadingCode,
                     LadingId = subNewShopping.LadingId,
                     PurchaseOrderId = subNewShopping.PurchaseOrderId,
                     PurchaseOrderCode = subNewShopping.PurchaseOrderCode,
                     Qty = payRequest.QualityControl.Qty,
                     UnitId = payRequest.QualityControl.UnitId,
                     UnitName = payRequest.QualityControl.Unit.Name,
                     UnitPrice = subNewShopping.UnitPrice,
                     TotalPrice =
                                 Math.Round(
                                     subNewShopping.UnitPrice * payRequest.QualityControl.QualityControlSummary.FailedQty ??
                                     0, 5),
                     CurrencyId = subNewShopping.CurrencyId,
                     CurrencyTitle = subNewShopping.CurrencyTitle,
                     FinancialAccountCode = subNewShopping.FinancialAccountCode,
                     RowVersion = payRequest.RowVersion
                   };
      return result;
    }
    #endregion
    #region Filter
    public IQueryable<PayRequestResult> FilterPayRequestResult(
        IQueryable<PayRequestResult> query,
        TValue<int> providerId = null,
        TValue<int> storeReceiptId = null,
        TValue<string> storeReceiptCode = null,
        TValue<int> stuffId = null)
    {
      if (providerId != null)
        query = query.Where(i => i.ProviderId == providerId);
      if (storeReceiptId != null)
        query = query.Where(i => i.StoreReceiptId == storeReceiptId);
      if (storeReceiptCode != null)
        query = query.Where(i => i.StoreReceiptCode == storeReceiptCode);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      return query;
    }
    #endregion
    #region Search
    public IQueryable<PayRequestResult> SearchPayRequestResult(
        IQueryable<PayRequestResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                    item.QualityControlCode.Contains(searchText) ||
                    item.PurchaseOrderCode.Contains(searchText) ||
                    item.CargoItemCode.Contains(searchText) ||
                    item.LadingCode.Contains(searchText) ||
                    item.CargoCode.Contains(searchText) ||
                    item.Description.Contains(searchText)
                select item;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<PayRequestResult> SortPayRequestResult(
       IQueryable<PayRequestResult> query,
       SortInput<PayRequestSortType> sort)
    {
      switch (sort.SortType)
      {
        case PayRequestSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case PayRequestSortType.QualityControlCode:
          return query.OrderBy(a => a.QualityControlCode, sort.SortOrder);
        case PayRequestSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case PayRequestSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case PayRequestSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case PayRequestSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case PayRequestSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case PayRequestSortType.UnitPrice:
          return query.OrderBy(a => a.UnitPrice, sort.SortOrder);
        case PayRequestSortType.TotalPrice:
          return query.OrderBy(a => a.TotalPrice, sort.SortOrder);
        case PayRequestSortType.CurrencyTitle:
          return query.OrderBy(a => a.CurrencyTitle, sort.SortOrder);
        case PayRequestSortType.ProviderName:
          return query.OrderBy(a => a.ProviderName, sort.SortOrder);
        case PayRequestSortType.CargoItemCode:
          return query.OrderBy(a => a.CargoItemCode, sort.SortOrder);
        case PayRequestSortType.PurchaseOrderCode:
          return query.OrderBy(a => a.PurchaseOrderCode, sort.SortOrder);
        case PayRequestSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case PayRequestSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}