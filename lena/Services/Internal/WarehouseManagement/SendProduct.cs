using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.SendProduct;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Common;
using lena.Models.StaticData;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region AddSendProduct
    public SendProduct AddSendProduct(
        SendProduct sendProduct,
       TransactionBatch transactionBatch,
       int preparingSendingId,
       int exitReceiptId,
       string description)
    {

      sendProduct = sendProduct ?? repository.Create<SendProduct>();
      var preparingSending = GetPreparingSending(preparingSendingId);
      sendProduct.PreparingSending = preparingSending;
      sendProduct.ExitReceiptId = exitReceiptId;
      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: sendProduct,
                  transactionBatch: transactionBatch,
                  description: description);
      return sendProduct;
    }
    #endregion
    #region EditSendProduct
    public SendProduct EditSendProduct(
        int id,
        byte[] rowVersion,
        TValue<int> preparingSendingId = null,
        TValue<int> exitReceiptId = null,
        TValue<string> description = null,
        TValue<bool> isDelete = null)
    {

      SendProduct sendProduct = GetSendProduct(id: id);
      return EditSendProduct(
                sendProduct: sendProduct,
                description: description,
                exitReceiptId: exitReceiptId,
                preparingSendingId: preparingSendingId,
                rowVersion: rowVersion,
                isDelete: isDelete); ;
    }
    public SendProduct EditSendProduct(
        SendProduct sendProduct,
        byte[] rowVersion,
        TValue<int> preparingSendingId = null,
        TValue<int> exitReceiptId = null,
        TValue<string> description = null,
        TValue<bool> isDelete = null)
    {

      if (preparingSendingId != null)
        sendProduct.PreparingSending.Id = preparingSendingId;
      if (exitReceiptId != null)
        sendProduct.ExitReceiptId = exitReceiptId;
      if (description != null)
        sendProduct.Description = description;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: sendProduct,
                    description: description,
                    isDelete: isDelete,
                    rowVersion: rowVersion);
      return retValue as SendProduct;
    }
    #endregion
    #region EditSendProductPrice
    public SendProduct EditSendProductPriceProccess(
        int id,
        TValue<int> priceAnnunciationItemId = null
        )
    {

      SendProduct sendProduct = GetSendProduct(id: id);
      PriceAnnunciationItem priceAnnunciationItem = null;
      var accounting = App.Internals.Accounting;
      if (priceAnnunciationItemId != null)
      {
        priceAnnunciationItem = App.Internals.SaleManagement.GetPriceAnnunciationItem(id: priceAnnunciationItemId);
      }
      sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.PriceAnnunciationItem = priceAnnunciationItem;
      sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.PriceAnnunciationItemId = priceAnnunciationItem?.Id;
      ExitReceiptRequestType sendProductExitReceiptRequestType = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.ExitReceiptRequestType;
      int? financialTransactionTypeId = null;
      if (sendProductExitReceiptRequestType.Id == StaticExitReceiptRequestTypes.DisposalOfWasteExitReceiptRequest.Id)
        financialTransactionTypeId = StaticFinancialTransactionTypes.SaleOfWaste.Id;
      else if (sendProductExitReceiptRequestType.Id == StaticExitReceiptRequestTypes.GivebackExitReceiptRequest.Id)
        financialTransactionTypeId = StaticFinancialTransactionTypes.Giveback.Id;
      else
        throw new CantAddExitReceiptRequestPriceException(sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.ExitReceiptRequestType.Title);
      ///حذف تراکنش های مالی قبلی
      if (sendProduct.FinancialTransactionBatch != null)
      {
        var financialTransactions = accounting.GetFinancialTransactions(selector: e => e, financialTransactionBatchId: sendProduct.FinancialTransactionBatch.Id);
        foreach (var financialTransaction in financialTransactions)
        {
          accounting.DeleteFinancialTransaction(financialTransaction);
        }
      }
      #region AddFinancialTransaction
      var cooperatorId = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.CooperatorId;
      var cooperatorFinancialAccounts = accounting.GetCooperatorFinancialAccounts(selector: e => e, cooperatorId: cooperatorId, currencyId: sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.PriceAnnunciationItem.CurrencyId);
      if (!cooperatorFinancialAccounts.Any())
      {
        throw new NotDefinedCooperatorFinancialAccountException(sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Cooperator.Name);
      }
      var cooperatorFinancialAccount = cooperatorFinancialAccounts.FirstOrDefault();
      #region AddFinancialTransactionBatch
      var financialTransactionBatch = accounting.AddFinancialTransactionBatch();
      #endregion
      var financialTransactionType = accounting.GetFinancialTransactionType(financialTransactionTypeId.Value);
      App.Internals.Accounting.AddFinancialTransactionProcess(
                    financialTransaction: null,
                    amount: priceAnnunciationItem.Price,
                    effectDateTime: DateTime.Now.ToUniversalTime(),
                    description: financialTransactionType.Description,
                    financialAccountId: cooperatorFinancialAccount.Id,
                    financialTransactionType: financialTransactionType,
                    financialTransactionBatchId: financialTransactionBatch.Id,
                    referenceFinancialTransaction: null);
      #endregion
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
              financialTransactionBatch: financialTransactionBatch,
              baseEntity: sendProduct,
              rowVersion: sendProduct.RowVersion);
      return retValue as SendProduct;
    }
    #endregion
    #region Get
    public SendProduct GetSendProduct(int id) => GetSendProduct(selector: e => e, id: id);
    public TResult GetSendProduct<TResult>(
        Expression<Func<SendProduct, TResult>> selector,
        int id)
    {

      var sendProduct = GetSendProducts(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (sendProduct == null)
        throw new SendProductNotFoundException(id);
      return sendProduct;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetSendProducts<TResult>(
        Expression<Func<SendProduct, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> preparingSendingId = null,
        TValue<int> sendPermissionId = null,
        TValue<int?> exitReceiptId = null,
        TValue<int?> stuffId = null,
        TValue<int?> cooperatorId = null,
        TValue<bool?> exitReceiptconfirmed = null
        )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<SendProduct>();
      if (exitReceiptId != null)
        query = query.Where(r => r.ExitReceiptId == exitReceiptId);
      if (preparingSendingId != null)
        query = query.Where(r => r.PreparingSending.Id == preparingSendingId);
      if (sendPermissionId != null)
        query = query.Where(r => r.PreparingSending.SendPermissionId == sendPermissionId);
      if (stuffId != null)
        query = query.Where(r => r.PreparingSending.SendPermission.ExitReceiptRequest.StuffId == stuffId);
      if (cooperatorId != null)
        query = query.Where(r => r.ExitReceipt.CooperatorId == cooperatorId);
      if (exitReceiptconfirmed != null)
        query = query.Where(r => r.ExitReceipt.Confirmed == exitReceiptconfirmed);
      if (description != null)
        query = query.Where(i => i.Description == description);
      return query.Select(selector);
    }
    public IQueryable<TResult> GetSendProductsComboResult<TResult>(
       Expression<Func<SendProduct, TResult>> selector,
       TValue<int> id = null,
         TValue<string> code = null,
         TValue<bool> isDelete = null,
         TValue<int> userId = null,
         TValue<int> transactionBatchId = null,
         TValue<string> description = null,
         TValue<int> preparingSendingId = null,
         TValue<int> sendPermissionId = null,
         TValue<int?> exitReceiptId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<SendProduct>();
      if (exitReceiptId != null)
        query = query.Where(r => r.ExitReceiptId == exitReceiptId);
      if (preparingSendingId != null)
        query = query.Where(r => r.PreparingSending.Id == preparingSendingId);
      if (sendPermissionId != null)
        query = query.Where(r => r.PreparingSending.SendPermissionId == sendPermissionId);
      if (description != null)
        query = query.Where(i => i.Description == description);
      return query.Select(selector);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<SendProductResult> SortSendProductResult(IQueryable<SendProductResult> query, SortInput<SendProductSortType> sort)
    {
      switch (sort.SortType)
      {
        case SendProductSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case SendProductSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case SendProductSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case SendProductSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case SendProductSortType.CooperatorCode:
          return query.OrderBy(a => a.CooperatorCode, sort.SortOrder);
        case SendProductSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case SendProductSortType.ExitReceiptRequestCode:
          return query.OrderBy(a => a.ExitReceiptRequestCode, sort.SortOrder);
        case SendProductSortType.Address:
          return query.OrderBy(a => a.Address, sort.SortOrder);
        case SendProductSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case SendProductSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case SendProductSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case SendProductSortType.TransportDateTime:
          return query.OrderBy(a => a.TransportDateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToSendProductResult
    //public SendProductResult ToSendProductResult(SendProduct sendProduct)
    //{
    //    var preparingSendingId = sendProduct.PreparingSending;
    //    var sendPermission = preparingSendingId.SendPermission;
    //    var sendProduct = sendPermission.OrderItemBlock;
    //    var orderItem = orderItemBlock.OrderItem;
    //    var stuff = orderItem.Stuff;
    //    var result = new SendProductResult
    //    {
    //        Id = sendPermission.Id,
    //        OrderItemId = orderItemBlock.OrderItemId,
    //        OrderItemQty = orderItemBlock.Qty,
    //        OrderItemBlockId = sendPermission.OrderItemBlockId,
    //        OrderItemBlockQty = orderItemBlock.Qty,
    //        OrderItemBlockUnitId = orderItemBlock.UnitId,
    //        OrderItemBlockUnitName = orderItemBlock.Unit.Name,
    //        StuffId = orderItem.StuffId,
    //        StuffCode = stuff.Code,
    //        StuffName = stuff.Name,
    //        Qty = sendPermission.Qty,
    //        UnitId = sendPermission.UnitId,
    //        UnitName = sendPermission.Unit.Name,
    //        DateTime = sendPermission.DateTime,
    //        Confirmed = sendPermission.Confirmed,
    //        RowVersion = sendPermission.RowVersion
    //    };
    //    return result;
    //}
    //public IQueryable<SendPermissionResult> ToSendPermissionResultQuery(IQueryable<SendPermission> query)
    //{
    //    return from sendPermission in query
    //        let orderItemBlock = sendPermission.OrderItemBlock
    //        let orderItem = orderItemBlock.OrderItem
    //        let stuff = orderItem.Stuff
    //        select new SendPermissionResult
    //        {
    //            Id = sendPermission.Id,
    //            OrderItemId = orderItemBlock.OrderItemId,
    //            OrderItemQty = orderItemBlock.Qty,
    //            OrderItemBlockId = sendPermission.OrderItemBlockId,
    //            OrderItemBlockQty = orderItemBlock.Qty,
    //            OrderItemBlockUnitId = orderItemBlock.UnitId,
    //            OrderItemBlockUnitName = orderItemBlock.Unit.Name,
    //            StuffId = orderItem.StuffId,
    //            StuffCode = stuff.Code,
    //            StuffName = stuff.Name,
    //            Qty = sendPermission.Qty,
    //            UnitId = sendPermission.UnitId,
    //            UnitName = sendPermission.Unit.Name,
    //            DateTime = sendPermission.DateTime,
    //            Confirmed = sendPermission.Confirmed,
    //            RowVersion = sendPermission.RowVersion
    //        };
    //}
    #endregion
    #region ToFullResult
    public Expression<Func<SendProduct, SendProductFullResult>> ToSendProductFullResult =
        sendProduct => new SendProductFullResult
        {
          Id = sendProduct.Id,
          ExitReceiptId = sendProduct.ExitReceiptId,
          Code = sendProduct.Code,
          PreparingSendingId = sendProduct.PreparingSending.Id,
          PreparingSendingCode = sendProduct.PreparingSending.Code,
          SendPermissionId = sendProduct.PreparingSending.SendPermissionId,
          SendPermissionCode = sendProduct.PreparingSending.SendPermission.Code,
          CooperatorId = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.CooperatorId,
          CooperatorCode = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Cooperator.Code,
          CooperatorName = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Cooperator.Name,
          OrderItemId = (sendProduct.PreparingSending.SendPermission.ExitReceiptRequest as OrderItemBlock).OrderItemId,
          OrderItemCode = (sendProduct.PreparingSending.SendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.Code,
          OrderItemUnitId = (sendProduct.PreparingSending.SendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.UnitId,
          OrderItemUnitName = (sendProduct.PreparingSending.SendPermission.ExitReceiptRequest as OrderItemBlock).OrderItem.Unit.Name,
          OrderItemQty = (sendProduct.PreparingSending.SendPermission.ExitReceiptRequest as OrderItemBlock).Qty,
          ExitReceiptRequestId = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Id,
          ExitReceiptRequestQty = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Qty,
          ExitReceiptRequestUnitId = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.UnitId,
          ExitReceiptRequestUnitName = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Unit.Name,
          ExitReceiptRequestTypeId = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.ExitReceiptRequestTypeId,
          ExitReceiptRequestTypeTitle = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.ExitReceiptRequestType.Title,
          StuffId = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.StuffId,
          StuffCode = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Stuff.Code,
          StuffName = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Stuff.Name,
          SendPermissionQty = sendProduct.PreparingSending.SendPermission.Qty,
          SendPermissionUnitId = sendProduct.PreparingSending.SendPermission.UnitId,
          SendPermissionUnitName = sendProduct.PreparingSending.SendPermission.Unit.Name,
          Qty = sendProduct.PreparingSending.Qty,
          UnitId = sendProduct.PreparingSending.UnitId,
          UnitName = sendProduct.PreparingSending.Unit.Name,
          DateTime = sendProduct.DateTime,
          Status = sendProduct.PreparingSending.Status,
          RowVersion = sendProduct.PreparingSending.RowVersion
        };
    public Expression<Func<SendProduct, SendProductComboResult>> ToSendProductComboResult =
       sendProduct => new SendProductComboResult
       {
         Id = sendProduct.Id,
         Code = sendProduct.Code,
         CooperatorId = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.CooperatorId,
         CooperatorName = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Cooperator.Name,
         CooperatorCode = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Cooperator.Code,
         RowVersion = sendProduct.PreparingSending.RowVersion
       };
    #endregion
    #region AddSendProductProcess
    public SendProduct AddSendProductProcess(
        TransactionBatch transactionBatch,
        int preparingSendingId,
        int exitReceiptId,
        string description)
    {

      #region AddTransactionBatch
      transactionBatch = transactionBatch ?? AddTransactionBatch();
      #endregion
      #region AddSendProduct
      var sendProduct = AddSendProduct(
              sendProduct: null,
              transactionBatch: null,
              preparingSendingId: preparingSendingId,
              exitReceiptId: exitReceiptId,
              description: description);
      #endregion
      #region SendPreparingSending
      var preparingSending = SendPreparingSending(preparingSending: sendProduct.PreparingSending,
          rowVersion: sendProduct.PreparingSending.RowVersion);
      #endregion
      #region Get BlockedTransactions
      var blockedTransactions = GetWarehouseTransactions(
              selector: e => e,
              transactionBatchId: preparingSending.TransactionBatch.Id,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportBlock.Id);
      #endregion
      #region AddTransactions
      foreach (var blockedTransaction in blockedTransactions)
      {
        if (blockedTransaction.WarehouseId == null)
          throw new BaseTransactionHasNoWarehouseException(id: blockedTransaction.Id);
        var serialBillofMaterialVersion = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                                  stuffId: blockedTransaction.StuffId,
                                  stuffSerialCode: blockedTransaction.StuffSerialCode
                                  );
        #region Export ExportBlockedFromWarehouse Transaction
        var exportFromBlockedTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
            transactionBatchId: transactionBatch.Id,
            effectDateTime: transactionBatch.DateTime,
            stuffId: blockedTransaction.StuffId,
            billOfMaterialVersion: serialBillofMaterialVersion,
            stuffSerialCode: blockedTransaction.StuffSerialCode,
            warehouseId: blockedTransaction.WarehouseId.Value,
            transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportBlockedFromWarehouse.Id,
            amount: blockedTransaction.Amount,
            unitId: blockedTransaction.UnitId,
            description: null,
            referenceTransaction: blockedTransaction);
        #endregion
      }
      #endregion
      #region GetProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: preparingSendingId,
              scrumTaskType: ScrumTaskTypes.SendProduct);
      #endregion
      //check projectWork not null
      if (projectWorkItem != null)
      {
        #region DoneTask
        App.Internals.ScrumManagement.DoneScrumTask(
                scrumTask: projectWorkItem,
                rowVersion: projectWorkItem.RowVersion);
        #endregion
      }
      return sendProduct;
    }
    #endregion
    #region ToResult
    public Expression<Func<SendProduct, SendProductResult>> ToSendProductResult =
        sendProduct => new SendProductResult
        {
          Id = sendProduct.Id,
          Code = sendProduct.Code,
          RowVersion = sendProduct.RowVersion,
          Description = sendProduct.Description,
          CooperatorId = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.CooperatorId,
          CooperatorCode = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Cooperator.Code,
          Qty = sendProduct.PreparingSending.Qty,
          CooperatorName = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Cooperator.Name,
          UnitId = sendProduct.PreparingSending.UnitId,
          StuffId = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.StuffId,
          ExitReceiptRequestId = sendProduct.PreparingSending.SendPermission.ExitReceiptRequestId,
          UnitName = sendProduct.PreparingSending.Unit.Name,
          StuffName = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Stuff.Name,
          StuffCode = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Stuff.Code,
          ExitReceiptRequestCode = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Code,
          Address = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.Address,
          DateTime = sendProduct.ExitReceipt.DateTime,
          TransportDateTime = sendProduct.ExitReceipt.OutboundCargo.TransportDateTime,
          PriceAnnunciationItemId = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.PriceAnnunciationItemId,
          PriceAnnunciation = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.PriceAnnunciationItem.Price,
          CurrencyName = sendProduct.PreparingSending.SendPermission.ExitReceiptRequest.PriceAnnunciationItem.Currency.Title
        };
    #endregion
    #region Delete
    public void DeleteSendProduct(int sendProductId)
    {

      var sendProduct = GetSendProduct(
                id: sendProductId);
      EditSendProduct(
                id: sendProduct.Id,
                rowVersion: sendProduct.RowVersion,
                isDelete: true);
    }
    public void RemoveSendProduct(int sendProductId)
    {

      var sendProduct = GetSendProduct(
                id: sendProductId);
      repository.Delete(sendProduct);
    }
    #endregion
  }
}