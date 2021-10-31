using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.Receipt;
using lena.Services.Common.Helpers;
//using System.Data.Entity;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public Receipt AddReceipt(
       Receipt receipt,
       TransactionBatch transactionBatch,
       int cooperatorId,
       DateTime receiptDateTime,
       ReceiptStatus status,
       StoreReceiptType receiptType,
       string description)
    {
      receipt = receipt ?? repository.Create<Receipt>();
      receipt.CooperatorId = cooperatorId;
      receipt.ReceiptDateTime = receiptDateTime;
      receipt.Status = status;
      receipt.ReceiptType = receiptType;
      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: receipt,
                transactionBatch: transactionBatch,
                  description: description);
      return receipt;
    }
    #endregion
    #region Edit
    public Receipt EditReceipt(
        int id,
        byte[] rowVersion,
        TValue<int> cooperatorId = null,
        TValue<ReceiptStatus> status = null,
        TValue<string> description = null)
    {
      Receipt receipt = GetReceipt(id: id);
      return EditReceipt(
                    receipt: receipt,
                    rowVersion: rowVersion,
                    cooperatorId: cooperatorId,
                    status: status,
                    description: description);
    }
    public Receipt EditReceipt(
        Receipt receipt,
        byte[] rowVersion,
        TValue<int> cooperatorId = null,
        TValue<ReceiptStatus> status = null,
        TValue<string> description = null)
    {
      if (cooperatorId != null)
        receipt.CooperatorId = cooperatorId;
      if (status != null)
        receipt.Status = status.Value;
      if (description != null)
        receipt.Description = description;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: receipt,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as Receipt;
    }
    #endregion
    #region Delete
    public void DeleteReceipt(int id)
    {
      var receipt = GetReceipt(id: id);
      #region DeleteStoreReceipt
      var storeReceipts = GetStoreReceipts(selector: e => e,
              receiptId: id,
              isDelete: false);
      foreach (var storeReceipt in storeReceipts)
      {
        var editStoreReceipt = EditStoreReceipt(
                      id: storeReceipt.Id,
                      receiptId: new TValue<int?>(null),
                      rowVersion: storeReceipt.RowVersion);
      }
      #endregion
      repository.Delete(receipt);
    }
    #endregion
    #region AcceptPriceState
    public Receipt AcceptReceiptPriceState(
        int id,
        byte[] rowVersion)
    {
      var receipt = GetReceipt(id: id);
      return AcceptReceiptPriceState(
                    receipt: receipt,
                    rowVersion: rowVersion);
    }
    public Receipt AcceptReceiptPriceState(
        Receipt receipt,
        byte[] rowVersion)
    {
      if (receipt.Status.HasFlag(ReceiptStatus.EternalReceipt))
        throw new ReceiptNotInEternalReceiptStatusException();
      var status = receipt.Status | ReceiptStatus.Priced;
      return EditReceipt(
                    receipt: receipt,
                    rowVersion: rowVersion,
                    status: status);
    }
    #endregion
    #region RejectPriceState
    public Receipt RejectReceiptPriceState(
        int id,
        byte[] rowVersion)
    {
      var receipt = GetReceipt(id: id);
      return RejectReceiptPriceState(
                    receipt: receipt,
                    rowVersion: rowVersion);
    }
    public Receipt RejectReceiptPriceState(
        Receipt receipt,
        byte[] rowVersion)
    {
      var status = receipt.Status & (~ReceiptStatus.Priced);
      return EditReceipt(
                    receipt: receipt,
                    rowVersion: rowVersion,
                    status: status);
    }
    #endregion
    #region Get
    public Receipt GetReceipt(int id) => GetReceipt(selector: e => e, id: id);
    public TResult GetReceipt<TResult>(
        Expression<Func<Receipt, TResult>> selector,
        int id)
    {
      var receipt = GetReceipts(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (receipt == null)
        throw new ReceiptNotFoundException(id);
      return receipt;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetReceipts<TResult>(
        Expression<Func<Receipt, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<string> ladingCode = null,
        TValue<string> cargoItemCode = null,
        TValue<string> purchaseOrderCode = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> cooperatorId = null,
        TValue<int> stuffId = null,
        TValue<ReceiptStatus> receiptStatus = null,
        TValue<ReceiptStatus[]> receiptStatuses = null,
        TValue<ReceiptStatus[]> receiptNotHasStatuses = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var receipt = baseQuery.OfType<Receipt>();
      //todo fix
      //if (inboundCargoId != null)
      //	receipt = receipt.Where(r => r.InboundCargoId == inboundCargoId);
      //if (warehouseId != null)
      //	receipt = receipt.Where(r => r.WarehouseId == warehouseId);
      //if (priceState != null)
      //	receipt = receipt.Where(r => r.PriceState == priceState);
      if (receiptStatus != null)
        receipt = receipt.Where(i => i.Status.HasFlag(receiptStatus));
      if (receiptStatuses != null)
      {
        var s = ReceiptStatus.None;
        foreach (var item in receiptStatuses.Value)
          s = s | item;
        receipt = receipt.Where(i => (i.Status & s) > 0);
      }
      if (receiptNotHasStatuses != null)
      {
        var s = ReceiptStatus.None;
        foreach (var item in receiptNotHasStatuses.Value)
          s = s | item;
        receipt = receipt.Where(i => (i.Status & s) == 0);
      }
      if (userId != null)
        receipt = receipt.Where(i => i.UserId == userId);
      if (stuffId != null)
      {
        //receipt = receipt.Where(i => i.StoreReceipts.Any(j => j.StuffId == stuffId));
        var receiptIds = GetStoreReceipts(
           selector: e => e.ReceiptId,
           stuffId: stuffId,
           isDelete: false)
           .Distinct();
        receipt = from item in receipt
                  join rId in receiptIds on item.Id equals rId
                  select item;
      }
      if (cooperatorId != null)
        receipt = receipt.Where(i => i.CooperatorId == cooperatorId);
      if (!string.IsNullOrWhiteSpace(ladingCode))
        receipt = receipt.Where(r => r.StoreReceipts.OfType<NewShopping>()
                               .Any(n => n.LadingItem.Lading.Code == ladingCode));
      if (!string.IsNullOrWhiteSpace(cargoItemCode))
        receipt = receipt.Where(r => r.StoreReceipts.OfType<NewShopping>()
                               .Any(n => n.LadingItem.CargoItem.Code == cargoItemCode));
      if (!string.IsNullOrWhiteSpace(purchaseOrderCode))
        receipt = receipt.Where(r => r.StoreReceipts.OfType<NewShopping>()
                               .Any(n => n.LadingItem.CargoItem.PurchaseOrder.Code == purchaseOrderCode));
      if (ids != null)
      {
        receipt = receipt.Where(r => ids.Value.Contains(r.Id));
      }
      return receipt.Select(selector);
    }
    #endregion
    #region AddProcess
    public void AddReceiptProcess(
        int cooperatorId,
        string description,
        DateTime receiptDateTime,
        AddReceiptItemInput[] storeReceipts)
    {
      StoreReceiptType receiptType = StoreReceiptType.NewShopping;
      #region CheckConditionToConvertReceipt
      if (storeReceipts.Any())
      {
        var ids = storeReceipts.Select(m => m.Id).ToArray();
        var selsectedStoreReceipt = App.Internals.WarehouseManagement.GetStoreReceipts(
                  selector: e => e,
                  ids: ids,
                  isDelete: false);
        receiptType = selsectedStoreReceipt.FirstOrDefault().StoreReceiptType;
        var count = selsectedStoreReceipt.Select(m => m.StoreReceiptType).Distinct().Count();
        if (count > 1)
        {
          throw new StoreReceiptTypeNotMatchException();
        }
        //باید تامین کننده کالاهای انتخاب شده یکی باشد 
        var providers = selsectedStoreReceipt.Where(m => m.CooperatorId != cooperatorId);
        if (providers.Any())
        {
          throw new TheSelectedStuffsProvidersAreNotSameException();
        }
        // باید تاریخ ورود کالا ها به انبار یکی باشد
        var dateCheck = selsectedStoreReceipt.Select(m => m.DateTime.Date).Distinct().Count();
        if (dateCheck > 1)
        {
          throw new TheDateOfArrivalOfTheStuffInTheWarehouseMustBeTheSameException();
        }
        //باید کالا های انتخاب شده تحت یک بارنامه باشند 
        var newShoppings = selsectedStoreReceipt.OfType<NewShopping>();
        var ladingCount = newShoppings.Select(m => m.LadingItem.LadingId).Distinct().Count();
        if (ladingCount > 1)
        {
          throw new TheStuffAreNotRelatedToSameLadingException();
        }
        // کالاها انتخاب شده باید وضعیت کنترل کیفی شان تعیین شده باشد
        var qaultiyControlStatus = selsectedStoreReceipt.SelectMany(m => m.ReceiptQualityControls).Where(m => m.Status == QualityControlStatus.NotAction);
        if (qaultiyControlStatus.Any())
        {
          var storeReceiptCode = qaultiyControlStatus.FirstOrDefault().StoreReceipt.Code;
          throw new ThereIsStuffWhoseQualityControlIsNotSpecifiedException(storeReceiptCode: storeReceiptCode);
        }
        var checkInternalProvider = App.Providers.Storage.CheckInternalProvider;
        var checkForeingProvider = App.Providers.Storage.CheckForeignProvider;
        if (checkInternalProvider || checkForeingProvider)
        {
          var cooperators = App.Internals.SaleManagement
                    .GetCooperators(
                        selector: e => e,
                        id: cooperatorId);
          var provider = cooperators.Where(i => i.CooperatorType == CooperatorType.Provider).FirstOrDefault();
          if (provider != null)
          {
            if ((provider.ProviderType == ProviderType.Internal && checkInternalProvider) || (provider.ProviderType == ProviderType.Foreign && checkForeingProvider))
            {
              var storeReciept = selsectedStoreReceipt.FirstOrDefault();
              //گرفتن کد بارنامه برای فیلتر کردن بقیه رسید ها
              var mainNewShopping = App.Internals.WarehouseManagement.GetNewShopping(id: storeReciept.Id);
              int ladingId = mainNewShopping.LadingItem.LadingId;
              // شرط باید از یک تامین کننده باشد لحاظ شده
              var allAcceptableStoreReceipt = App.Internals.WarehouseManagement.GetStoreReceipts(
                  e => e,
                  cooperatorId: cooperatorId,
                  isDelete: false);
              // فیلتر رسید های موقتی که رسید دائم ندارد
              allAcceptableStoreReceipt = allAcceptableStoreReceipt.Where(m => m.ReceiptId == null);
              // رسید های انتخاب شده را استثنا می کنیم 
              allAcceptableStoreReceipt = allAcceptableStoreReceipt.Where(m => !ids.Contains(m.Id));
              // شرط تاریخ ورود به انبار یکی باشد لحاظ شده
              allAcceptableStoreReceipt = allAcceptableStoreReceipt.Where(m => m.DateTime.Date == storeReciept.DateTime.Date);
              foreach (var item in allAcceptableStoreReceipt)
              {
                var newShopping = App.Internals.WarehouseManagement.GetNewShoppings(e => e, receiptId: item.Id);
                var checkLading = newShopping.Where(m => m.LadingItem.LadingId != ladingId);
                if (!checkLading.Any())
                {
                  throw new YouMustAddAllItemsMatchingTheSelectedItemsToTheListException(code: item.Code);
                }
              }
            }
          }
        }
      }
      #endregion
      #region AddReceipt
      var receipt = AddReceipt(
              receipt: null,
              transactionBatch: null,
              cooperatorId: cooperatorId,
              receiptDateTime: receiptDateTime,
              receiptType: receiptType,
              status: ReceiptStatus.Temporary,
              description: description);
      #endregion
      #region EditStoreReceipt set ReceiptId  
      foreach (var addStoreReceipt in storeReceipts)
      {
        #region Check CooperatorNotMatch And StoreReceiptTypeNotMatch
        var storeReceipt = GetStoreReceipt(id: addStoreReceipt.Id);
        if (storeReceipt.CooperatorId != cooperatorId)
          throw new CooperatorNotMatchException();
        if (storeReceipt.StoreReceiptType != receiptType)
          throw new StoreReceiptTypeNotMatchException();
        #endregion
        var editStoreReceipt = EditStoreReceipt(
                storeReceipt: storeReceipt,
                receiptId: receipt.Id,
                rowVersion: storeReceipt.RowVersion);
      }
      #endregion
    }
    #endregion
    #region EditProcess
    public void EditReceiptProcess(
        int id,
        int cooperatorId,
        string description,
        byte[] rowVersion,
        AddReceiptItemInput[] addStoreReceipts,
        DeleteReceiptItemInput[] deleteStoreReceipts)
    {
      #region GetReceipt
      var receipt = GetReceipt(id: id);
      #endregion
      #region Check receipt status for edit 
      if (receipt.Status.HasFlag(ReceiptStatus.EternalReceipt))
        throw new EditReceiptInEternalReceiptStatusNotAllowedException();
      #endregion
      #region EditReceipt
      EditReceipt(
                      receipt: receipt,
                      rowVersion: rowVersion,
                      description: description,
                      cooperatorId: cooperatorId);
      #endregion
      #region EditStoreReceipt set ReceiptId
      foreach (var addStoreReceipt in addStoreReceipts)
      {
        #region Check CooperatorNotMatch And StoreReceiptTypeNotMatch
        var storeReceipt = GetStoreReceipt(id: addStoreReceipt.Id);
        if (storeReceipt.CooperatorId != cooperatorId)
          throw new CooperatorNotMatchException();
        if (storeReceipt.StoreReceiptType != receipt.ReceiptType)
          throw new StoreReceiptTypeNotMatchException();
        #endregion
        var editStoreReceipt = EditStoreReceipt(
                storeReceipt: storeReceipt,
                receiptId: receipt.Id,
                rowVersion: storeReceipt.RowVersion);
      }
      #endregion
      #region DeleteStoreReceipt set ReceiptId Null
      foreach (var editStoreReceipt in deleteStoreReceipts)
      {
        #region Check CooperatorNotMatch
        var storeReceipt = GetStoreReceipt(id: editStoreReceipt.Id);
        if (storeReceipt.CooperatorId != cooperatorId)
          throw new CooperatorNotMatchException();
        if (storeReceipt.StoreReceiptType != receipt.ReceiptType)
          throw new StoreReceiptTypeNotMatchException();
        #endregion
        EditStoreReceipt(
                storeReceipt: storeReceipt,
                receiptId: new TValue<int?>(null),
                rowVersion: storeReceipt.RowVersion);
      }
      #endregion
    }
    #endregion
    #region SetEternalReceipt
    public Receipt SetEternalReceipt(
        int id,
        byte[] rowVersion)
    {
      var receipt = GetReceipt(id: id);
      return SetEternalReceipt(
                    receipt: receipt,
                    rowVersion: rowVersion);
    }
    public Receipt SetEternalReceipt(
        Receipt receipt,
        byte[] rowVersion)
    {
      if (receipt.Status.HasFlag(ReceiptStatus.EternalReceipt))
        throw new ReceiptIsInEternalReceiptStatusException(id: receipt.Id);
      #region check qualityControls
      var qualityControls = App.Internals.QualityControl.GetReceiptQualityControls(
          selector: e => e,
          isDelete: false,
          status: QualityControlStatus.NotAction,
          receiptId: receipt.Id);
      if (qualityControls.Any())
        throw new ReceiptHasNotActionQualityControlException();
      #endregion
      EditReceipt(
              receipt: receipt,
              rowVersion: rowVersion,
              status: ReceiptStatus.EternalReceipt);
      return receipt;
    }
    #endregion
    #region Search
    public IQueryable<ReceiptResult> SearchReceiptResult(
        IQueryable<ReceiptResult> query,
        StoreReceiptType? storeReceiptType,
        string search,
        DateTime? fromDate,
        DateTime? toDate,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(search))
        query = query.Where(item =>
            item.Code.Contains(search) ||
            item.EmployeeFullName.Contains(search) ||
            item.CooperatorCode.Contains(search) ||
            item.CooperatorName.Contains(search));
      if (fromDate != null)
        query = query.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.DateTime <= toDate);
      if (storeReceiptType != null) query =
          query.Where(i => i.StoreReceiptType == storeReceiptType);
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    public IQueryable<ReceiptResult> SearchReceiptFullResult(
        IQueryable<ReceiptResult> query,
        string search,
        DateTime? fromDate,
        DateTime? toDate)
    {
      if (!string.IsNullOrEmpty(search))
        query = query.Where(item =>
            item.Code.Contains(search) ||
            item.EmployeeFullName.Contains(search) ||
            item.CooperatorCode.Contains(search) ||
            item.CooperatorName.Contains(search));
      if (fromDate != null)
        query = query.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.DateTime <= toDate);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ReceiptResult> SortReceiptResult(
        IQueryable<ReceiptResult> query,
        SortInput<ReceiptSortType> sort)
    {
      switch (sort.SortType)
      {
        case ReceiptSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ReceiptSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ReceiptSortType.ReceiptStatus:
          return query.OrderBy(a => a.ReceiptStatus, sort.SortOrder);
        case ReceiptSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case ReceiptSortType.ReceiptDateTime:
          return query.OrderBy(a => a.ReceiptDateTime, sort.SortOrder);
        case ReceiptSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case ReceiptSortType.CooperatorCode:
          return query.OrderBy(a => a.CooperatorCode, sort.SortOrder);
        case ReceiptSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case ReceiptSortType.LadingCode:
          return query.OrderBy(a => a.LadingCode, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<ReceiptFullResult> SortReceiptFullResult(
        IQueryable<ReceiptFullResult> query,
        SortInput<StoreReceiptSortType> sort)
    {
      switch (sort.SortType)
      {
        //case StoreReceiptSortType.InboundCargoCode:
        //    return query.OrderBy(a => a.InboundCargoCode, sort.SortOrder);
        //case StoreReceiptSortType.CooperatorName:
        //    return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        //case StoreReceiptSortType.StoreReceiptType:
        //    return query.OrderBy(a => a.StoreReceiptType, sort.SortOrder);
        //case StoreReceiptSortType.Amount:
        //    return query.OrderBy(a => a.Amount, sort.SortOrder);
        //case StoreReceiptSortType.UnitName:
        //    return query.OrderBy(a => a.UnitName, sort.SortOrder);
        //case StoreReceiptSortType.BoxNo:
        //    return query.OrderBy(a => a.BoxNo, sort.SortOrder);
        //case StoreReceiptSortType.QtyPerBox:
        //    return query.OrderBy(a => a.QtyPerBox, sort.SortOrder);
        //case StoreReceiptSortType.DateTime:
        //    return query.OrderBy(a => a.DateTime, sort.SortOrder);
        //case StoreReceiptSortType.ReceiptCode:
        //    return query.OrderBy(a => a.ReceiptCode, sort.SortOrder);
        //case StoreReceiptSortType.Status:
        //    return query.OrderBy(a => a.ReceiptStatus, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToReceiptResult
    public Expression<Func<Receipt, ReceiptResult>> ToReceiptResult =
        receipt => new ReceiptResult
        {
          Id = receipt.Id,
          Code = receipt.Code,
          DateTime = receipt.DateTime,
          ReceiptDateTime = receipt.ReceiptDateTime,
          StoreReceiptType = receipt.ReceiptType,
          ReceiptStatus = receipt.Status,
          RowVersion = receipt.RowVersion,
          CooperatorId = receipt.CooperatorId,
          CooperatorCode = receipt.Cooperator.Code,
          CooperatorName = receipt.Cooperator.Name,
          EmployeeFullName = receipt.User.Employee.FirstName + "  " + receipt.User.Employee.LastName,
          UserId = receipt.UserId,
          LadingCode = receipt.StoreReceipts.OfType<NewShopping>().FirstOrDefault().LadingItem.Lading.Code
        };
    public Expression<Func<Receipt, ReceiptFullResult>> ToReceiptFullResult =
        entity =>
             new ReceiptFullResult
             {
               Id = entity.Id,
               ReceiptCode = entity.Code,
               DateTime = entity.DateTime,
               ReceiptStatus = entity.Status,
               RowVersion = entity.RowVersion,
               CooperatorId = entity.CooperatorId,
               CooperatorCode = entity.Cooperator.Code,
               CooperatorName = entity.Cooperator.Name,
               Description = entity.Description,
               EmployeeFullName = entity.User.Employee.FirstName + "  " + entity.User.Employee.LastName,
               UserId = entity.UserId,
               StoreReceipts = entity.StoreReceipts.OfType<NewShopping>().AsQueryable().Select(App.Internals.WarehouseManagement.NewShoppingToStoreReceiptResult)
                                .Union(
                entity.StoreReceipts.OfType<ReturnStoreReceipt>().AsQueryable().Select(App.Internals.WarehouseManagement.ReturnStoreReceiptToStoreReceiptResult)
                ) //entity.StoreReceipts.OfType<NewShopping>().AsQueryable().Select(App.Internals.WarehouseManagement.NewShoppingToStoreReceiptResult)
               //todo fix
               //.Union(entity.StoreReceipts.OfType<ReturnStoreReceipt>().AsQueryable().Select(App.Internals.WarehouseManagement.ReturnOfSaleToStoreReceiptResult))
             };
    #endregion
  }
}