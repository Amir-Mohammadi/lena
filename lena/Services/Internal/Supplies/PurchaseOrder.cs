using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.PurchaseOrder;
using System.Collections.Generic;
using lena.Services.Common.Helpers;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Accounting.FinancialDocument;
using lena.Models.Supplies.PurchaseOrderDetail;
using lena.Models.Supplies.PurchaseRequest;
using lena.Models.ApplicationBase.BaseEntityDocument;
using lena.Models.ApplicationBase.Unit;
//using LinqLib.Operators;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region ValidateAddPurchaseOrders
    public IQueryable<ValidationSaveCargoResult> ValidateSaveCargo(
       SelectedOrder[] selectedOrders)
    {

      var addPurchaseOrder = (from item in selectedOrders
                              group item by new
                              {
                                StuffId = item.StuffId,
                                StuffCode = item.StuffCode,
                                StuffName = item.StuffName
                              } into g
                              select new
                              {
                                StuffId = g.Key.StuffId,
                                stuffCode = g.Key.StuffCode,
                                StuffName = g.Key.StuffName,
                                PurchaseOrderIds = g.Select(d => d.Id).ToArray()
                              }).ToArray();
      var result = new List<ValidationSaveCargoResult>();
      foreach (var item in addPurchaseOrder)
      {
        var s = PurchaseOrderStatus.Cargoed;
        var purchaseOrders = GetPurchaseOrders(e => e, stuffId: item.StuffId, isDelete: false)

              .Where(i => (i.Status & s) == 0 && !(item.PurchaseOrderIds.Contains(i.Id))).OrderBy(i => i.Deadline).FirstOrDefault();
        foreach (var ids in item.PurchaseOrderIds)
        {
          if (purchaseOrders != null)
          {
            DateTime minDeadLine = purchaseOrders.Deadline;
            int id = ids;
            var purchaseOrder = GetPurchaseOrder(id: id);
            var deadLine = purchaseOrder.Deadline;
            if (deadLine > minDeadLine)
            {
              var catResult = new ValidationSaveCargoResult()
              {
                StuffId = purchaseOrder.StuffId,
                IsValid = false,
                Deadline = minDeadLine,
                StuffCode = item.stuffCode,
                StuffName = item.StuffName
              };
              result.Add(catResult);
              break;
            }
            else
              continue;
          }
        }
      }
      return result.AsQueryable();
    }
    #endregion
    #region ValidateAddPurchaseOrders
    public IQueryable<ValidationAddPurchaseOrderResult> ValidateAddPurchaseOrders(
       AddPurchaseOrderInput[] addPurchaseOrders)
    {

      var addPurchaseOrder = (from item in addPurchaseOrders
                              group item by new
                              {
                                StuffId = item.StuffId,
                                StuffCode = item.StuffCode
                              } into g
                              select new
                              {
                                StuffId = g.Key.StuffId,
                                StuffCode = g.Key.StuffCode,
                                PurchaseRequestIds = g.Select(d => d.PurchaseRequestId).ToArray()
                              }).ToArray();
      var result = new List<ValidationAddPurchaseOrderResult>();
      foreach (var item in addPurchaseOrder)
      {
        var s = PurchaseRequestStatus.Ordered;
        var purchaseRequests = GetPurchaseRequests(e => e, stuffId: item.StuffId, isDelete: false)

              .Where(i => (i.Status & s) == 0 && !(item.PurchaseRequestIds.Contains(i.Id))).OrderBy(i => i.Deadline).FirstOrDefault();
        foreach (var ids in item.PurchaseRequestIds)
        {
          if (purchaseRequests != null)
          {
            DateTime minDeadLine = purchaseRequests.Deadline;
            int id = ids;
            var purchaseRequest = GetPurchaseRequest(id: id);
            var deadLine = purchaseRequest.Deadline;
            if (deadLine > minDeadLine)
            {
              var catResult = new ValidationAddPurchaseOrderResult()
              {
                StuffId = purchaseRequest.StuffId,
                StuffCode = item.StuffCode,
                IsValid = false,
                Deadline = minDeadLine
              };
              result.Add(catResult);
              break;
            }
            else
              continue;
          }
        }
      }
      return result.AsQueryable();
    }
    #endregion
    #region Adds Process
    public PurchaseOrder[] AddPurchaseOrdersProcess(
        AddFinancialDocumentInput financialDocumentCost,
        AddFinancialDocumentInput financialDocumentDiscount,
        AddBaseEntityDocumentInput document,
        AddPurchaseOrderInput[] addPurchaseOrders,
        PurchaseOrderType purchaseOrderType)
    {

      var purchaseOrders = new List<PurchaseOrder>();
      var addPurchaseOrder = from item in addPurchaseOrders
                             group item by new
                             {
                               ProviderId = item.ProviderId,
                               SupplierId = item.SupplierId,
                               UnitId = item.UnitId,
                               Price = item.Price,
                               StuffId = item.StuffId,
                               Deadline = item.Deadline,
                               CurrencyId = item.CurrencyId,
                               CurrentStuffBasePrice = item.CurrentStuffBasePrice,
                               CurrentStuffBasePriceCurrencyId = item.CurrentStuffBasePriceCurrencyId,
                               StuffPriceDiscrepancyDescription = item.StuffPriceDiscrepancyDescription,
                               OrderInvoiceNum = item.OrderInvoiceNum,
                               PurchaseOrderDateTime = item.PurchaseOrderDateTime,
                               OrderInvoiceNumber = item.OrderInvoiceNum,
                               PurchaseOrderGroupId = item.PurchaseOrderGroupId
                             } into g
                             select new
                             {
                               ProviderId = g.Key.ProviderId,
                               SupplierId = g.Key.SupplierId,
                               Price = g.Key.Price,
                               UnitId = g.Key.UnitId,
                               StuffId = g.Key.StuffId,
                               Deadline = g.Key.Deadline,
                               CurrencyId = g.Key.CurrencyId,
                               CurrentStuffBasePrice = g.Key.CurrentStuffBasePrice,
                               CurrentStuffBasePriceCurrencyId = g.Key.CurrentStuffBasePriceCurrencyId,
                               StuffPriceDiscrepancyDescription = g.Key.StuffPriceDiscrepancyDescription,
                               OrderInvoiceNum = g.Key.OrderInvoiceNum,
                               PurchaseOrderDateTime = g.Key.PurchaseOrderDateTime,
                               PurchaseOrderGroupId = g.Key.PurchaseOrderGroupId,
                               PurchaseOrderPreapringDateTime = g.Select(r => r.PurchaseOrderPreparingDateTime).FirstOrDefault(),
                               items = g.Select(d => new
                               {
                                 Qty = d.Qty,
                                 UnitId = d.UnitId,
                                 PurchaseRequestId = d.PurchaseRequestId
                               })
                             };
      foreach (var item in addPurchaseOrder)
      {
        if (item.PurchaseOrderDateTime.ToLocalTime().Date > DateTime.Now.Date)
          throw new PurchaseOrderDateTimeCanNotGreaterThanCurrentDateException(DateTime.Now, item.PurchaseOrderDateTime);
        #region Compare PurchaseOrderQty with RemainedPurhaseRequsetQty
        var currentQtyItemInput = item.items.Select(m => new CurrentQtyItemInput
        {
          CurrentQty = m.Qty,
          CurrentUnitId = m.UnitId
        }).ToArray();
        var targetQtyItemsInput = new List<TargetQtyItemInput>();
        item.items.ForEach(m =>
              {
                var purchaseRequestId = m.PurchaseRequestId;
                var purchaseRequest = GetPurchaseRequest(id: purchaseRequestId);
                var remainPurchaseRequestQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.OrderedQty;
                var targetQtyItemInput = new TargetQtyItemInput();
                targetQtyItemInput.TargetQty = remainPurchaseRequestQty;
                targetQtyItemInput.TargetUnitId = purchaseRequest.UnitId;
                targetQtyItemsInput.Add(targetQtyItemInput);
              });
        var qtyItemCompareInput = new QtyItemCompareInput();
        qtyItemCompareInput.CurrentQtyItemInput = currentQtyItemInput;
        qtyItemCompareInput.TargetQtyItemInput = targetQtyItemsInput.ToArray();
        App.Internals.ApplicationBase.CompareQty(qtyItemCompareInput);
        #endregion
        ProjectWork projectWork = null;
        #region Calculate SumQty
        var sumQtyItemInput = item.items.Select(i => new SumQtyItemInput
        {
          Qty = i.Qty,
          UnitId = i.UnitId
        }).ToArray();
        var sumQty = App.Internals.ApplicationBase.SumQty(targetUnitId: item.UnitId, sumQtys: sumQtyItemInput);
        #endregion
        #region calculate price
        #endregion
        #region AddTransactionBatch
        var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
        #endregion
        #region Add StuffProvider If Not Exist
        if (item.ProviderId != null)
        {
          var stuffProvider = GetStuffProviders(stuffId: item.StuffId, providerId: item.ProviderId.Value)
                    .FirstOrDefault();
          if (stuffProvider == null)
          {
            stuffProvider = AddStuffProvider(
                            description: null,
                            stuffId: item.StuffId,
                            providerId: item.ProviderId.Value,
                            leadTime: 0,
                            instantLeadTime: 0,
                            isActive: true,
                            isDefault: false);
          }
        }
        #endregion
        #region AddPurchaseOrder
        var purchaseOrder = AddPurchaseOrderProcess(
                purchaseOrder: null,
                purchaseOrderGroupId: item.PurchaseOrderGroupId,
                transactionBatch: transactionBatch,
                providerId: item.ProviderId,
                supplierId: item.SupplierId,
                price: item.Price,
                currencyId: item.CurrencyId,
                orderInvoiceNum: item.OrderInvoiceNum,
                deadline: item.Deadline,
                purchaseOrderDateTime: item.PurchaseOrderDateTime,
                purchaseOrderPreparingDateTime: item.PurchaseOrderPreapringDateTime,
                qty: sumQty.Qty,
                unitId: sumQty.UnitId,
                stuffId: item.StuffId,
                description: null,
                purchaseOrderType: purchaseOrderType);
        #endregion
        #region AddStuffPriceDiscrepancy
        if (item.CurrentStuffBasePrice == null || item.Price != item.CurrentStuffBasePrice)
        {
          var stuffPriceDiscrepancy = App.Internals.Planning.AddStuffPriceDiscrepancy(
                    purchaseOrderId: purchaseOrder.Id,
                    purchaseOrderPrice: purchaseOrder.Price,
                    purchaseOrderCurrencyId: purchaseOrder.Currency.Id,
                    purchaseOrderQty: purchaseOrder.Qty,
                    currentStuffBasePrice: item.CurrentStuffBasePrice,
                    CurrentStuffBasePriceCurrencyId: item.CurrentStuffBasePriceCurrencyId,
                    description: item.StuffPriceDiscrepancyDescription
                    );
        }
        #endregion
        #region Add EstimatedPurchasePrice
        if (purchaseOrderType == PurchaseOrderType.ProviderOrder &&
            (item.CurrencyId == null || item.Price == null))
          throw new AddEstimatedPurchasePriceArgumentNullException();
        if (item.CurrencyId != null && item.Price != null)
        {
          var estimatedPurchasePrice = AddEstimatedPurchasePriceProcess(
                      currencyId: item.CurrencyId.Value,
                      price: item.Price.Value,
                      stuffId: item.StuffId,
                      purchaseOrderId: purchaseOrder.Id);
        }
        #endregion
        #region Add PurchaseOrderDetail
        foreach (var purchase in item.items)
        {
          #region GetPurchaseRequest
          var purchaseRequest = GetPurchaseRequest(e => e, id: purchase.PurchaseRequestId);
          #endregion
          #region GetProjectWork
          ScrumTask projectWorkItem = null;
          projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                       baseEntityId: purchase.PurchaseRequestId,
                       scrumTaskType: ScrumTaskTypes.PurchaseOrder);
          if (projectWorkItem == null)
            projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                          baseEntityId: purchase.PurchaseRequestId,
                          scrumTaskType: ScrumTaskTypes.PurchaseOrder);
          if (projectWork == null && projectWorkItem != null)
            projectWork = projectWorkItem.ScrumBackLog as ProjectWork;
          #endregion
          #region Get PruchaseRequest and PurchaseRequestTransactionPlan
          var purchaseRequestTransaction =
              App.Internals.WarehouseManagement.GetTransactionPlans(
                  selector: e => e,
                  transactionBatchId: purchaseRequest.TransactionBatch.Id,
                  isDelete: false)


                  .SingleOrDefault();
          #endregion
          #region Add PurchaseOrderDetailProcess
          var sumDetailQtyItemInput = new List<SumQtyItemInput>();
          var sumDetailQtyItemInputObj = new SumQtyItemInput();
          sumDetailQtyItemInputObj.Qty = purchase.Qty;
          sumDetailQtyItemInputObj.UnitId = purchase.UnitId;
          sumDetailQtyItemInput.Add(sumDetailQtyItemInputObj);
          var sumTargetQty = App.Internals.ApplicationBase.SumQty(
                    targetUnitId: purchase.UnitId,
                    sumQtys: sumDetailQtyItemInput.ToArray());
          #region Check PurchaseOrderQty
          if (purchaseRequest.PurchaseRequestSummary.OrderedQty + (sumTargetQty.Qty * sumTargetQty.ConvertRatio) / purchaseRequest.Unit.ConversionRatio > purchaseRequest.Qty)
          {
            throw new PurchaseOrderQtyCanNotBiggerThanPurchaseRequestQty(purchaseRequest.Id, purchaseRequest.Qty, purchaseOrder.Qty);
          }
          #endregion
          AddPurchaseOrderDetailProcess(
                  purchaseRequestId: purchaseRequest.Id,
                  purchaseOrderId: purchaseOrder.Id,
                  stuffId: item.StuffId,
                  qty: sumTargetQty.Qty,
                  unitId: sumTargetQty.UnitId,
                  purchaseRequestTransaction: purchaseRequestTransaction,
                  deadline: item.Deadline);
          #endregion
          #region  Done PurchaseOrderTask If Ordered
          if (purchaseRequest.PurchaseRequestSummary.OrderedQty > purchaseRequest.Qty)
          {
            if (projectWorkItem != null)
            {
              #region DoneTask
              App.Internals.ScrumManagement.DoneScrumTask(
                      scrumTask: projectWorkItem,
                      rowVersion: projectWorkItem.RowVersion);
              #endregion
            }
          }
          #endregion
        }
        #endregion
        #region Add ShippingTask
        #region Get DescriptionForTask
        var stuffNoun = purchaseOrder.Stuff.Noun;
        var stuffCode = purchaseOrder.Stuff.Code;
        #endregion
        if (projectWork != null)
          App.Internals.ProjectManagement.AddProjectWorkItem(
                        projectWorkItem: null,
                        name: $"ثبت محموله سفارش خرید {purchaseOrder.Code} ",
                        description: $"عنوان کالا:{stuffNoun},کدکالا:{stuffCode}",
                        color: "",
                        departmentId: (int)Departments.Supplies,
                        estimatedTime: 10800,
                        isCommit: false,
                        scrumTaskTypeId: (int)ScrumTaskTypes.Shipping,
                        userId: purchaseOrder.UserId,
                        spentTime: 0,
                        remainedTime: 0,
                        scrumTaskStep: ScrumTaskStep.ToDo,
                        projectWorkId: projectWork.Id,
                        baseEntityId: purchaseOrder.Id);
        #endregion
        purchaseOrders.Add(purchaseOrder);
      }
      if (financialDocumentCost != null)
      {
        App.Internals.Accounting.AddFinancialDocumentProcess(
                      financialDocument: null,
                      financialTransactionBatch: null,
                      debitAmount: financialDocumentCost.DebitAmount,
                      creditAmount: financialDocumentCost.CreditAmount,
                      financialAccountId: financialDocumentCost.FinancialAccountId,
                      uploadFileData: App.Providers.Session.GetAs<UploadFileData>(financialDocumentCost.FileKey),
                      financialDocumentTransfer: null,
                      financialDocumentBeginning: null,
                      financialDocumentCost: financialDocumentCost.FinancialDocumentCost,
                      financialDocumentCorrection: null,
                      financialDocumentDiscount: null,
                      financialDocumentBankOrder: null,
                      type: financialDocumentCost.Type,
                      description: financialDocumentCost.Description,
                      documentDate: financialDocumentCost.DocumentDate);
      }
      if (financialDocumentDiscount != null)
      {
        App.Internals.Accounting.AddFinancialDocumentProcess(
                      financialDocument: null,
                      financialTransactionBatch: null,
                      debitAmount: financialDocumentDiscount.DebitAmount,
                      creditAmount: financialDocumentDiscount.CreditAmount,
                      financialAccountId: financialDocumentDiscount.FinancialAccountId,
                      uploadFileData: App.Providers.Session.GetAs<UploadFileData>(financialDocumentDiscount.FileKey),
                      financialDocumentTransfer: null,
                      financialDocumentBeginning: null,
                      financialDocumentCost: null,
                      financialDocumentCorrection: null,
                      financialDocumentDiscount: financialDocumentDiscount.FinancialDocumentDiscount,
                      financialDocumentBankOrder: null,
                      type: financialDocumentDiscount.Type,
                      description: financialDocumentDiscount.Description,
                      documentDate: financialDocumentDiscount.DocumentDate);
      }
      if (document != null)
      {
        UploadFileData uploadFileData = null;
        if (!string.IsNullOrWhiteSpace(document.FileKey))
          uploadFileData = App.Providers.Session.GetAs<UploadFileData>(document.FileKey);
        if (uploadFileData == null)
          throw new DocumentIsNullException();
        App.Internals.ApplicationBase.AddBaseEntityDocument(
                     description: document.Description,
                     baseEntityId: document.BaseEntityId,
                     uploadFileData: uploadFileData,
                     baseEntityDocumentTypeId: document.BaseEntityDocumentTypeId,
                     cooperatorId: document.CooperatorId,
                     baseEntityDocumentIds: document.BaseEntityDocumentIds);
      }
      return purchaseOrders.ToArray();
    }
    #endregion
    #region Add Process
    public PurchaseOrder AddPurchaseOrderProcess(
        PurchaseOrder purchaseOrder,
        int purchaseOrderGroupId,
        TransactionBatch transactionBatch,
        int? providerId,
        int? supplierId,
        double? price,
        byte? currencyId,
        string orderInvoiceNum,
        DateTime deadline,
        DateTime purchaseOrderDateTime,
        DateTime purchaseOrderPreparingDateTime,
        double qty,
        byte unitId,
        int stuffId,
        string description,
        PurchaseOrderType purchaseOrderType)
    {

      FinancialTransactionBatch financialTransactionBatch = null;
      if (providerId == null) throw new ProviderIsNotDefinedException();
      if (price == null) throw new PurchasePriceIsNotDefinedException();
      #region FinancialTransactionBatch
      financialTransactionBatch = App.Internals.Accounting.AddFinancialTransactionBatch();
      #endregion
      var cooperatorFinancialAccount = App.Internals.Accounting.GetCooperatorFinancialAccounts(
              selector: e => e,
              cooperatorId: providerId.Value,
              currencyId: currencyId)


          .FirstOrDefault();
      if (cooperatorFinancialAccount == null)
        throw new CooperatorHasNoFinancialAccountException(cooperatorId: providerId.Value);
      App.Internals.Accounting.AddFinancialTransactionProcess(
                    financialTransaction: null,
                    amount: price.Value * qty,
                    effectDateTime: purchaseOrderDateTime,
                    description: null,
                    financialAccountId: cooperatorFinancialAccount.Id,
                    financialTransactionIsPermanent: false,
                    financialTransactionType: Models.StaticData.StaticFinancialTransactionTypes.ImportToPurchaseOrder,
                    financialTransactionBatchId: financialTransactionBatch.Id,
                    referenceFinancialTransaction: null);
      #region AddPurchaseOrder
      purchaseOrder = AddPurchaseOrder(
          purchaseOrder: purchaseOrder,
          purchaseOrderGroupId: purchaseOrderGroupId,
          transactionBatch: transactionBatch,
          financialTransactionBatch: financialTransactionBatch,
          providerId: providerId,
          supplierId: (supplierId == 0 ? null : supplierId),
          price: price,
          currencyId: currencyId,
          deadline: deadline,
          purchaseOrderDateTime: purchaseOrderDateTime,
          orderInvoiceNum: orderInvoiceNum,
          purchaseOrderPreparingDateTime: purchaseOrderPreparingDateTime,
          qty: qty,
          unitId: unitId,
          stuffId: stuffId,
          description: description,
          purchaseOrderType: purchaseOrderType);
      #endregion
      #region AddPurchaseOrderSummary
      AddPurchaseOrderSummary(
          cargoedQty: 0,
          receiptedQty: 0,
          qualityControlPassedQty: 0,
          qualityControlFailedQty: 0,
          purchaseOrderId: purchaseOrder.Id);
      #endregion
      return purchaseOrder;
    }
    #endregion
    #region Add
    public PurchaseOrder AddPurchaseOrder(
       PurchaseOrder purchaseOrder,
       int purchaseOrderGroupId,
       TransactionBatch transactionBatch,
       FinancialTransactionBatch financialTransactionBatch,
       int? providerId,
       int? supplierId,
       double? price,
       byte? currencyId,
       DateTime deadline,
       DateTime purchaseOrderDateTime,
       string orderInvoiceNum,
       DateTime purchaseOrderPreparingDateTime,
       double qty,
       byte unitId,
       int stuffId,
       string description,
       PurchaseOrderType purchaseOrderType)
    {

      //todo check null
      //if (purchaseOrderType == PurchaseOrderType.ProviderOrder)
      //{
      //}
      purchaseOrder = purchaseOrder ?? repository.Create<PurchaseOrder>();
      purchaseOrder.PurchaseOrderGroupId = purchaseOrderGroupId;
      purchaseOrder.ProviderId = providerId;
      purchaseOrder.Price = price;
      purchaseOrder.CurrencyId = currencyId;
      purchaseOrder.Deadline = deadline;
      purchaseOrder.PurchaseOrderDateTime = purchaseOrderDateTime;
      purchaseOrder.OrderInvoiceNum = orderInvoiceNum;
      purchaseOrder.PurchaseOrderPreparingDateTime = purchaseOrderPreparingDateTime;
      purchaseOrder.Qty = qty;
      purchaseOrder.UnitId = unitId;
      purchaseOrder.StuffId = stuffId;
      purchaseOrder.SupplierId = supplierId;
      purchaseOrder.Status = PurchaseOrderStatus.NotAction;
      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: purchaseOrder,
                transactionBatch: transactionBatch,
                financialTransactionBatch: financialTransactionBatch,
                description: description);
      return purchaseOrder;
    }
    #endregion
    #region PurchaseOrderGroup
    public PurchaseOrder AddToPurchaseOrderGroup(int id, byte[] rowVersion, int purchaseOrderGroupId)
    {

      var purchaseOrder = GetPurchaseOrder(id);
      if (purchaseOrder.PurchaseOrderGroupId.HasValue)
      {
        throw new PurchaseOrderIsInAnotherGroupException(
                  purchaseOrderCode: purchaseOrder.Code,
                  purchaseOrderId: purchaseOrder.Id,
                  purchaseOrderGroupId: purchaseOrder.PurchaseOrderGroupId.Value);
      }
      purchaseOrder.PurchaseOrderGroupId = purchaseOrderGroupId;
      repository.Update(purchaseOrder, rowVersion);
      return purchaseOrder;
    }
    public PurchaseOrder RemoveFromPurchaseOrderGroup(int id, byte[] rowVersion)
    {

      var purchaseOrder = GetPurchaseOrder(id);
      if (!purchaseOrder.PurchaseOrderGroupId.HasValue)
      {
        return purchaseOrder;
      }
      purchaseOrder.PurchaseOrderGroupId = null;
      repository.Update(purchaseOrder, rowVersion);
      return purchaseOrder;
    }
    #endregion
    #region EditProcess
    public PurchaseOrder EditPurchaseOrderProcess(
        int id,
        byte[] rowVersion,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<double> price = null,
        TValue<byte> currencyId = null,
        TValue<int> providerId = null,
        TValue<int> supplierId = null,
        TValue<DateTime> deadline = null,
        TValue<double> orderedQty = null,
        TValue<PurchaseOrderType> type = null,
        TValue<PurchaseOrderStatus> status = null,
        PurchaseOrderDetailInput[] purchaseOrderDetail = null,
        AddPurchaseOrderInput[] newAddedPurchaseOrders = null,
        TValue<bool> isArchived = null)
    {

      foreach (var newAddedpurchaseOrder in newAddedPurchaseOrders)
      {
        AddPurchaseOrderDetailProcess(
                      purchaseRequestId: newAddedpurchaseOrder.PurchaseRequestId,
                      purchaseOrderId: id,
                      stuffId: newAddedpurchaseOrder.StuffId,
                      qty: newAddedpurchaseOrder.Qty,
                      unitId: newAddedpurchaseOrder.UnitId,
                      purchaseRequestTransaction: null,
                      deadline: newAddedpurchaseOrder.Deadline);
        if (newAddedpurchaseOrder.OrderedQty > newAddedpurchaseOrder.Qty)
          throw new PurchaseOrderQtyCannotIncreaseException(id: id);
      }
      #region GetPurchaseOrder
      var purchaseOrder = GetPurchaseOrder(id: id);
      var previousPurchaseOrderId = purchaseOrder.Id;
      #endregion
      #region GetUnit
      var unit = App.Internals.ApplicationBase.GetUnit(id: unitId);
      #endregion
      //برای افزایش سفارش جدید ثبت شود.
      var decreasedQty = purchaseOrder.Qty * purchaseOrder.Unit.ConversionRatio - qty * unit.ConversionRatio;
      //if (decreasedQty < 0)
      //    throw new PurchaseOrderQtyCannotIncreaseException(id: id);
      if (qty < purchaseOrder.PurchaseOrderSummary.ReceiptedQty)
        throw new PurchaseOrderQtyCannotDecreaseException(id: id);
      var cargoItems = GetCargoItems(selector: e => e, purchaseOrderId: id, isDelete: false);
      var cargoedQty = cargoItems.Any() ? cargoItems.Sum(i => i.Qty * i.Unit.ConversionRatio) : 0;
      if (cargoedQty > qty * unit.ConversionRatio && decreasedQty > 0)
        throw new PurchaseOrderHasMoreCargoedException(id: id);
      var receiptQty = purchaseOrder.PurchaseOrderSummary.ReceiptedQty;
      if (receiptQty > 0)
      {
        if (qty < receiptQty)
          throw new PurchaseOrderHasMoreReceiptException(purchaseOrder.Id);
        if ((supplierId != null && purchaseOrder.SupplierId != supplierId) || (providerId != null && purchaseOrder.ProviderId != providerId))
          throw new CanNotChangePurchaseOrderSupplierWhenHasReceiptException(purchaseOrder.Id, receiptQty, supplierId);
      }
      //Add New Financial Transactions
      // Add StuffProvider If Not Exist
      if (providerId != null)
      {
        var stuffProvider = GetStuffProviders(stuffId: purchaseOrder.StuffId, providerId: providerId)
                  .FirstOrDefault();
        if (stuffProvider == null)
        {
          stuffProvider = AddStuffProvider(
                          description: null,
                          stuffId: purchaseOrder.StuffId,
                          providerId: providerId,
                          leadTime: 0,
                          instantLeadTime: 0,
                          isActive: true,
                          isDefault: false);
        }
      }
      #region Check Provider Has FinancialAccount
      var cooperatorFinancialAccount = App.Internals.Accounting.GetCooperatorFinancialAccounts(
              selector: e => e,
              cooperatorId: providerId.Value,
              currencyId: currencyId)


          .FirstOrDefault();
      if (cooperatorFinancialAccount == null)
      {
        throw new CooperatorHasNoFinancialAccountException(providerId);
      }
      #endregion
      if (purchaseOrder.FinancialTransactionBatch != null
          && (purchaseOrder.Price != price ||
              purchaseOrder.Qty != qty ||
              purchaseOrder.ProviderId != providerId ||
              purchaseOrder.UnitId != unitId ||
              purchaseOrder.SupplierId != supplierId ||
              purchaseOrder.CurrencyId != currencyId))
      {
        #region EditFinancialTransaction For PurchaseOrder
        var purchaseOrderFinancialTransactionBatch = purchaseOrder.FinancialTransactionBatch;
        if (purchaseOrderFinancialTransactionBatch == null)
          throw new PurchaseOrderHasNoFinancialTransactionException(purchaseOrderCode: purchaseOrder.Code);
        var financialTransactions = purchaseOrderFinancialTransactionBatch.FinancialTransactions;
        // حذف تراکنش های مالی
        foreach (var item in financialTransactions)
        {
          App.Internals.Accounting.DeleteFinancialTransaction(item.Id);
        }
        #region AddFinancialTransaction For PurchaseOrder
        App.Internals.Accounting.AddFinancialTransactionProcess(
                financialTransaction: null,
                amount: price * qty,
                effectDateTime: purchaseOrder.PurchaseOrderDateTime,
                financialAccountId: cooperatorFinancialAccount.Id,
                financialTransactionType: Models.StaticData.StaticFinancialTransactionTypes.ImportToPurchaseOrder,
                financialTransactionBatchId: purchaseOrderFinancialTransactionBatch.Id,
                financialTransactionIsPermanent: false,
                referenceFinancialTransaction: null,
                description: null);
        #endregion
        #endregion
        #region EditFinancialTransaction For CargoItems
        // حذف ft  برای جزئیات محموله
        var cargoItemIds = App.Internals.Supplies.GetCargoItems(
            selector: e => e.Id,
            purchaseOrderId: purchaseOrder.Id)

          .ToList();
        foreach (var cargoItem in cargoItems)
        {
          //Edit CargoItem
          EditCargoItem(
                  id: cargoItem.Id,
                  purchaseOrderId: purchaseOrder.Id,
                  rowVersion: cargoItem.RowVersion);
          var cargoItemFinancialTransactionBatch = App.Internals.Accounting.GetFinancialTransactionBatch(
                 cargoItem.FinancialTransactionBatch.Id);
          var cargoItemFinancialTransactions = App.Internals.Accounting.GetFinancialTransactions(selector: e => e,
                    financialTransactionBatchId: cargoItemFinancialTransactionBatch.Id);
          foreach (var item in cargoItemFinancialTransactions)
          {
            // حذف تراکنش های مالی
            App.Internals.Accounting.DeleteFinancialTransaction(item.Id);
          }
          var cargoItemQty = ((Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount)) * cargoItem.Unit.ConversionRatio) / purchaseOrder.Unit.ConversionRatio;
          #region Add ExportFromOrder FinancialTransaction
          var exportFromPurchaseFinancialTransaction = App.Internals.Accounting.AddFinancialTransactionProcess(
                  financialTransaction: null,
                  amount: price * cargoItemQty,
                  effectDateTime: cargoItem.CargoItemDateTime,
                  description: null,
                  financialAccountId: cooperatorFinancialAccount.Id,
                  financialTransactionType: Models.StaticData.StaticFinancialTransactionTypes.ExportFromPurchase,
                  financialTransactionBatchId: cargoItem.FinancialTransactionBatch.Id,
                  referenceFinancialTransaction: null);
          #endregion
          #region AddFinancialTransaction For CargoItem
          var cargoItemFinancialTransaction = App.Internals.Accounting.AddFinancialTransactionProcess(
                  financialTransaction: null,
                  amount: price * cargoItemQty,
                  effectDateTime: cargoItem.CargoItemDateTime,
                  description: null,
                  financialAccountId: cooperatorFinancialAccount.Id,
                  financialTransactionType: Models.StaticData.StaticFinancialTransactionTypes.ImportToCargo,
                  financialTransactionBatchId: cargoItem.FinancialTransactionBatch.Id,
                  financialTransactionIsPermanent: false,
                  referenceFinancialTransaction: exportFromPurchaseFinancialTransaction);
          #endregion
        }
        #endregion
        var estimatedPurchasePrice = AddEstimatedPurchasePriceProcess(
         currencyId: currencyId.Value,
         price: price.Value,
         stuffId: purchaseOrder.StuffId,
         purchaseOrderId: purchaseOrder.Id);
      }
      //Edit Purchase Order
      EditPurchaseOrder(
              purchaseOrder: purchaseOrder,
              rowVersion: purchaseOrder.RowVersion,
              qty: qty,
              unitId: unitId,
              price: price,
              supplierId: supplierId,
              currencyId: currencyId,
              providerId: providerId,
              deadline: deadline,
              type: type,
              status: purchaseOrder.Status,
              financialTransactionBatch: purchaseOrder.FinancialTransactionBatch,
              isArchived: isArchived);
      for (int i = 0; i < purchaseOrderDetail.Length; i++)
      {
        var purchaseOrderDetailInput = purchaseOrderDetail[i];
        var purchaseOrderDetailResult = GetPurchaseOrderDetail(
                  id: purchaseOrderDetailInput.Id);
        if (purchaseOrderDetailInput.OrderedQty > purchaseOrderDetailInput.Qty)
          throw new PurchaseOrderQtyCannotIncreaseException(id: id);
        EditPurchaseOrderDetailProcess(
                  id: purchaseOrderDetailInput.Id,
                  rowVersion: purchaseOrderDetailInput.RowVersion,
                  qty: purchaseOrderDetailInput.Qty,
                  transactionBatch: null);
        ResetPurchaseOrderDetailSummaryByPurchaseOrderDetailId(purchaseOrderDetailInput.Id);
        //if (purchaseOrderDetailInput.PurchaseRequestId != null)
        //    ResetPurchaseRequestSummaryByPurchaseRequestId(item.PurchaseRequestId.Value);
      }
      //var purchaseOrderDetail = GetPurchaseOrderDetail(
      //    id: purchaseOrderDetail.Id)
      //foreach (var item in purchaseOrderDetails)
      //{
      //if (decreasedQty < item.Qty)
      //{
      //    EditPurchaseOrderDetailProcess(
      //        id: item.Id,
      //        rowVersion: item.RowVersion,
      //        qty: item.Qty - decreasedQty,
      //        transactionBatch: null)
      //    
      //;
      //    break;
      //}
      //else
      //{
      //    RemovePurchaseOrderDetailProcess(id: item.Id,
      //        rowVersion: item.RowVersion,
      //        transactionBatch: null)
      //        
      //;
      //    decreasedQty = decreasedQty - item.Qty;
      //}
      //ResetPurchaseOrderDetailSummaryByPurchaseOrderDetailId(item.Id);
      //        if (item.PurchaseRequestId != null)
      //            ResetPurchaseRequestSummaryByPurchaseRequestId(item.PurchaseRequestId.Value);
      //}
      App.Internals.Supplies.ResetPurchaseOrderStatus(purchaseOrderId: previousPurchaseOrderId);
      return purchaseOrder;
    }
    #endregion
    #region Edit
    public PurchaseOrder EditPurchaseOrder(
        PurchaseOrder purchaseOrder,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<bool> isDelete = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<double> price = null,
        TValue<int> supplierId = null,
        TValue<byte> currencyId = null,
        TValue<int> providerId = null,
        TValue<DateTime> deadline = null,
        TValue<PurchaseOrderType> type = null,
        TValue<PurchaseOrderStatus> status = null,
        TValue<int> purchaseOrderStepDetailId = null,
        FinancialTransactionBatch financialTransactionBatch = null,
        TValue<bool> isArchived = null,
        TValue<Risk> risk = null)
    {

      if (providerId != null)
        purchaseOrder.ProviderId = providerId;
      if (price != null)
        purchaseOrder.Price = price;
      if (currencyId != null)
        purchaseOrder.CurrencyId = currencyId;
      if (deadline != null)
        purchaseOrder.Deadline = deadline;
      if (qty != null)
        purchaseOrder.Qty = qty;
      if (unitId != null)
        purchaseOrder.UnitId = unitId;
      if (status != null)
        purchaseOrder.Status = status;
      if (supplierId != null)
        purchaseOrder.SupplierId = supplierId;
      if (type != null)
        purchaseOrder.PurchaseOrderType = type;
      if (isArchived != null)
        purchaseOrder.IsArchived = isArchived;
      if (purchaseOrderStepDetailId != null)
        purchaseOrder.PurchaseOrderStepDetailId = purchaseOrderStepDetailId;
      if (risk != null)
        purchaseOrder.LatestRisk = risk;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: purchaseOrder,
                    description: description,
                    isDelete: isDelete,
                    financialTransactionBatch: financialTransactionBatch,
                    rowVersion: rowVersion);
      return retValue as PurchaseOrder;
    }
    #endregion
    #region Get
    public PurchaseOrder GetPurchaseOrder(int id) => GetPurchaseOrder(e => e, id: id);
    public TResult GetPurchaseOrder<TResult>(
        Expression<Func<PurchaseOrder, TResult>> selector,
        int id)
    {

      var purchaseOrder = GetPurchaseOrders(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (purchaseOrder == null)
        throw new PurchaseOrderNotFoundException(id);
      return purchaseOrder;
    }
    public PurchaseOrder GetPurchaseOrder(string code) => GetPurchaseOrder(e => e, code: code);
    public TResult GetPurchaseOrder<TResult>(
        Expression<Func<PurchaseOrder, TResult>> selector,
        string code)
    {

      var purchaseOrder = GetPurchaseOrders(
                    selector: selector,
                    code: code)


                .FirstOrDefault();
      if (purchaseOrder == null)
        throw new PurchaseOrderNotFoundException(code);
      return purchaseOrder;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetPurchaseOrders<TResult>(
        Expression<Func<PurchaseOrder, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<int> purchaseOrderGroupId = null,
        TValue<int?[]> purchaseOrderGroupIds = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int?> providerId = null,
        TValue<ProviderType> providerType = null,
        TValue<int?> employeeId = null,
        TValue<int?> supplierId = null,
        TValue<PurchaseOrderType> purchaseOrderType = null,
        TValue<StuffType> stuffType = null,
        TValue<double?> price = null,
        TValue<int?> currencyId = null,
        TValue<DateTime> deadline = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int> cargoId = null,
        TValue<int> stuffId = null,
        TValue<int> financialTransactionBatchId = null,
        TValue<int> financialDocumentId = null,
        TValue<FinancialDocumentTypeResult> financialDocumentTypeResult = null,
        TValue<int[]> selectedPlanCodeIds = null,
        TValue<string> purchaseRequsetDescription = null,
        TValue<PurchaseOrderStatus> status = null,
        TValue<PurchaseOrderStatus[]> statuses = null,
        TValue<PurchaseOrderStatus[]> notHasStatuses = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<bool> isArchived = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<PurchaseOrder>();
      if (providerId != null)
        query = query.Where(r => r.ProviderId == providerId);
      if (stuffType != null)
        query = query.Where(r => r.Stuff.StuffType == stuffType);
      if (employeeId != null)
        query = query.Where(r => r.User.Employee.Id == employeeId);
      if (supplierId != null)
        query = query.Where(r => r.SupplierId == supplierId);
      if (purchaseOrderType != null)
        query = query.Where(r => r.PurchaseOrderType == purchaseOrderType);
      if (price != null)
        query = query.Where(r => r.Price == price);
      if (currencyId != null)
        query = query.Where(r => r.CurrencyId == currencyId);
      if (deadline != null)
        query = query.Where(r => r.Deadline == deadline);
      if (qty != null)
        query = query.Where(r => Math.Abs(r.Qty - qty) < 0.000001);
      if (unitId != null)
        query = query.Where(r => r.UnitId == unitId);
      if (stuffId != null)
        query = query.Where(r => r.StuffId == stuffId);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (purchaseOrderGroupId != null)
        query = query.Where(i => i.PurchaseOrderGroupId == purchaseOrderGroupId);
      //todo 5 change query
      if (cargoId != null)
        query = query.Where(i => i.PurchaseOrderDetails.Any(j => j.CargoItemDetails.Any(n => n.CargoItem.CargoId == cargoId)));
      if (status != null)
        query = query.Where(i => i.Status.HasFlag(status));
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));
      if (purchaseOrderGroupIds != null)
        query = query.Where(i => purchaseOrderGroupIds.Value.Contains(i.PurchaseOrderGroupId));
      if (financialTransactionBatchId != null)
        query = query.Where(i => i.FinancialTransactionBatch.Id == financialTransactionBatchId);
      if (financialDocumentId != null && financialDocumentTypeResult != null)
      {
        if (financialDocumentTypeResult == FinancialDocumentTypeResult.PurchaseOrderDiscount)
          query = query.Where(i => i.PurchaseOrderDiscounts.Select(d => d.FinancialDocumentDiscount.FinancialDocument.Id).Contains(financialDocumentId));
        else if (financialDocumentTypeResult == FinancialDocumentTypeResult.PurchaseOrderCost)
          query = query.Where(i => i.PurchaseOrderCosts.Select(d => d.FinancialDocumentCost.FinancialDocument.Id).Contains(financialDocumentId));
      }
      if (isArchived != null)
        query = query.Where(i => i.IsArchived == isArchived);
      if (purchaseRequsetDescription != null && purchaseRequsetDescription != "")
      {
        var purchaseOrderDetails = App.Internals.Supplies.GetPurchaseOrderDetails(e => e);
        var purchaseOrderIds = (from purchaseOrderDetail in purchaseOrderDetails
                                where (purchaseOrderDetail.PurchaseRequest.Description.Contains(purchaseRequsetDescription))
                                select purchaseOrderDetail.PurchaseOrderId).Distinct();
        query = from q in query
                join purchaseOrderId in purchaseOrderIds
                      on q.Id equals purchaseOrderId
                select q;
      }
      if (selectedPlanCodeIds != null)
      {
        query = from item in query
                from purchaseOrderDetail in item.PurchaseOrderDetails
                where selectedPlanCodeIds.Value.Contains(purchaseOrderDetail.PurchaseRequest.PlanCodeId.Value)
                select item;
        query = query.Distinct();
      }
      if (statuses != null)
      {
        var s = PurchaseOrderStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        query = query.Where(i => (i.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = PurchaseOrderStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.Status & s) == 0);
      }
      if (fromDate != null)
        query = query.Where(r => r.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(r => r.DateTime <= toDate);
      if (providerType != null)
        query = query.Where(r => r.Provider.ProviderType == providerType);
      return query.Select(selector);
    }
    public IQueryable<PurchaseOrderPlanCodeView> GetPurchaseOrderPlanCodes(
      TValue<int> purchaseOrderId = null,
      TValue<string> planCodes = null)
    {

      var query = repository.GetQuery<PurchaseOrderPlanCodeView>();
      if (purchaseOrderId != null)
        query = query.Where(i => i.PurchaseOrderId <= purchaseOrderId);
      if (planCodes != null)
        query = query.Where(i => i.PlanCodes == planCodes);
      return query;
    }
    public IQueryable<PurchaseOrderResponsibleView> GetPurchaseOrderResponsibles(
     TValue<int> purchaseOrderId = null,
     TValue<string> responsibleFullNames = null)
    {

      var query = repository.GetQuery<PurchaseOrderResponsibleView>();
      if (purchaseOrderId != null)
        query = query.Where(i => i.PurchaseOrderId <= purchaseOrderId);
      if (responsibleFullNames != null)
        query = query.Where(i => i.ResponsibleFullNames == responsibleFullNames);
      return query;
    }
    #endregion
    #region ToPurchaseOrderResultQuery
    public IQueryable<PurchaseOrderResult> ToPurchaseOrderResultQuery(IQueryable<PurchaseOrder> query, IQueryable<PurchaseOrderDetail> purchaseOrderDetails,
        IQueryable<BaseEntityDocument> latestBaseEntityDocuments, IQueryable<BaseEntityConfirmation> purchaseOrderConfirmations, IQueryable<CargoItem> cargoItems,
        IQueryable<LadingItem> ladingItems, IQueryable<BankOrder> bankOrders, IQueryable<Lading> ladings, IQueryable<FinanceItem> financeItems, IQueryable<PurchaseOrderPlanCodeView> purchaseOrderPlanCodes)
    {
      var groupResult = from purchaseOrderDetail in purchaseOrderDetails
                        group purchaseOrderDetail by purchaseOrderDetail.PurchaseOrderId into g
                        select new
                        {
                          PurchaseOrderId = g.Key,
                          DescriptionArray = g.Select(i => i.PurchaseRequest.Description),
                          MinDeadlinePurchaseRequest = g.Select(i => i.PurchaseRequest.Deadline).Min()
                        };
      var cargoResult = from cargos in cargoItems
                        group cargos by cargos.PurchaseOrderId into car
                        select new
                        {
                          MaxEstimateDateTime = car.Select(i => i.EstimateDateTime).Max(),
                          PurchaseOrderId = car.Key
                        };
      var joinFourTabels = from bankOrder in bankOrders
                           join lading in ladings
                           on bankOrder.Id equals lading.BankOrderId into bankOrderLadings
                           from bankOrderLading in bankOrderLadings.DefaultIfEmpty()
                           join ladingItem in ladingItems
                           on bankOrderLading.Id equals ladingItem.LadingId into ladingItemBankOrderLadings
                           from ladingItembankOrderLading in ladingItemBankOrderLadings.DefaultIfEmpty()
                           join cargoItem in cargoItems
                           on ladingItembankOrderLading.CargoItemId equals cargoItem.Id into ladingItembankOrderLadingCargoItems
                           from ladingItembankOrderLadingCargoItem in ladingItembankOrderLadingCargoItems.DefaultIfEmpty()
                           join purchaseOrder in query
                           on ladingItembankOrderLadingCargoItem.PurchaseOrderId equals purchaseOrder.Id into purchaseOrderJoins
                           from purchaseOrderJoin in purchaseOrderJoins.DefaultIfEmpty()
                           select new
                           {
                             PurchaseOrderId = purchaseOrderJoin.Id,
                             BankOrderStatus = bankOrder.Status,
                             LadingItemStatus = ladingItembankOrderLading.Status,
                             CargoItemStatus = ladingItembankOrderLadingCargoItem.Status,
                           };
      var joinedGroups = from joinFourTabel in joinFourTabels
                         group joinFourTabel by joinFourTabel.PurchaseOrderId into groupedQuery
                         select new
                         {
                           Id = groupedQuery.Key,
                           BankOrderStatus = groupedQuery.Select(i => i.BankOrderStatus).Distinct(),
                           LadingItemStatus = groupedQuery.Select(i => i.LadingItemStatus).Distinct(),
                           CargoItemStatus = groupedQuery.Select(i => i.CargoItemStatus).Distinct()
                         };
      var groupedfinanceItems = from purchasOrderFinanceItem in financeItems
                                group purchasOrderFinanceItem by purchasOrderFinanceItem.PurchaseOrderId into g
                                select new
                                {
                                  PurchaseOrderId = g.Key,
                                  AllocatedAmount = g.Sum(x => x.AllocatedAmount)
                                };
      var resultQuery = from purchaseOrder in query
                        join g in groupResult on
                        purchaseOrder.Id equals g.PurchaseOrderId
                        join latestBaseEntityDocument in latestBaseEntityDocuments on
                        purchaseOrder.Id equals latestBaseEntityDocument.BaseEntityId
                        into tLatestBaseEntityDocuments
                        from latestBaseEntityDocument in tLatestBaseEntityDocuments.DefaultIfEmpty()
                        join purchaseConfirmation in purchaseOrderConfirmations on purchaseOrder.Id equals purchaseConfirmation.ConfirmingEntityId into leftJoinPurchaseConfirm
                        from priceConfirmQuery in leftJoinPurchaseConfirm.DefaultIfEmpty()
                        join cargos in cargoResult on purchaseOrder.Id equals cargos.PurchaseOrderId into cargoo
                        from cargoItem in cargoo.DefaultIfEmpty()
                        join groupedPurchaseOrderFinanceItem in groupedfinanceItems
                        on purchaseOrder.Id equals groupedPurchaseOrderFinanceItem.PurchaseOrderId into financeItem
                        from purchasOrderFinanceItem in financeItem.DefaultIfEmpty()
                          //join joinedGroup in joinedGroups
                          //on cargoItem.PurchaseOrderId equals joinedGroup.Id into joinedGroupCargoItems
                          //from joinedGroupCargoItem in joinedGroupCargoItems.DefaultIfEmpty()
                        join purchaseOrderPlanCode in purchaseOrderPlanCodes on
                          purchaseOrder.Id equals purchaseOrderPlanCode.PurchaseOrderId into tPurchaseOrderWithPlanCode
                        from purchaseOrderWithPlanCode in tPurchaseOrderWithPlanCode.DefaultIfEmpty()
                        select new PurchaseOrderResult
                        {
                          Id = purchaseOrder.Id,
                          Code = purchaseOrder.Code,
                          //BankOrderStatus = joinedGroupCargoItem.BankOrderStatus,
                          //LadingItemStatus = joinedGroupCargoItem.LadingItemStatus,
                          //CargoItemStatus = joinedGroupCargoItem.CargoItemStatus,
                          DateTime = purchaseOrder.DateTime,
                          SupplierId = purchaseOrder.SupplierId,
                          OrderInvoiceNum = purchaseOrder.OrderInvoiceNum,
                          SupplierFullName = purchaseOrder.Supplier.Employee.FirstName + " " + purchaseOrder.Supplier.Employee.LastName,
                          PurchaseOrderType = purchaseOrder.PurchaseOrderType,
                          ProviderId = purchaseOrder.ProviderId,
                          ProviderName = purchaseOrder.StuffProvider.Provider.Name,
                          ProviderCode = purchaseOrder.StuffProvider.Provider.Code,
                          StuffId = purchaseOrder.StuffId,
                          StuffCode = purchaseOrder.Stuff.Code,
                          StuffName = purchaseOrder.Stuff.Name,
                          StuffGrossWeight = purchaseOrder.Stuff.GrossWeight,
                          StuffType = purchaseOrder.Stuff.StuffType,
                          Price = purchaseOrder.Price,
                          TotalPrice = purchaseOrder.Price * purchaseOrder.Qty,
                          CurrencuyId = purchaseOrder.CurrencyId,
                          CurrencyTitle = purchaseOrder.Currency.Title,
                          CurrencySign = purchaseOrder.Currency.Sign,
                          Qty = purchaseOrder.Qty,
                          UnitId = purchaseOrder.UnitId,
                          UnitName = purchaseOrder.Unit.Name,
                          Deadline = purchaseOrder.Deadline,
                          PurchaseOrderDateTime = purchaseOrder.PurchaseOrderDateTime,
                          PurchaseOrderPreparingDateTime = purchaseOrder.PurchaseOrderPreparingDateTime,
                          Description = purchaseOrder.Description,
                          CargoedQty = purchaseOrder.PurchaseOrderSummary.CargoedQty,
                          QualityControlPassedQty = purchaseOrder.PurchaseOrderSummary.QualityControlPassedQty,
                          QualityControlFailedQty = purchaseOrder.PurchaseOrderSummary.QualityControlFailedQty,
                          ReceiptedQty = purchaseOrder.PurchaseOrderSummary.ReceiptedQty,
                          RemainedQty = purchaseOrder.Qty - purchaseOrder.PurchaseOrderSummary.CargoedQty,
                          StuffCategoryId = purchaseOrder.Stuff.StuffCategoryId,
                          StuffCategoryName = purchaseOrder.Stuff.StuffCategory.Name,
                          PurchaseOrderStatus = purchaseOrder.Status,
                          FinancialTransacionBatchId = purchaseOrder.FinancialTransactionBatch.Id,
                          EmployeeFullName = purchaseOrder.User.Employee.FirstName + " " + purchaseOrder.User.Employee.LastName,
                          PurchaseRequestDescriptionArray = g.DescriptionArray,
                          PlanCode = purchaseOrderWithPlanCode.PlanCodes,
                          LatestBaseEntityDocumentDescription = latestBaseEntityDocument.Description,
                          LatestBaseEntityDocumentDateTime = latestBaseEntityDocument.DateTime,
                          PriceConfirmationStatus = priceConfirmQuery.Status,
                          PriceConfirmerId = priceConfirmQuery.UserId,
                          PriceConfirmerFullName = priceConfirmQuery.User.Employee.FirstName + " " + priceConfirmQuery.User.Employee.LastName,
                          PriceConfirmDescription = priceConfirmQuery.ConfirmDescription,
                          PurchaseOrderGroupCode = purchaseOrder.PurchaseOrderGroup.Code,
                          PurchaseOrderGroupId = purchaseOrder.PurchaseOrderGroup.Id,
                          PurchaseOrderId = purchaseOrder.Id,
                          AllocatedAmount = purchasOrderFinanceItem.AllocatedAmount ?? 0,
                          RemainingAmount = (purchaseOrder.Price * purchaseOrder.Qty) - (purchasOrderFinanceItem.AllocatedAmount ?? 0),
                          FinanceAllocationStatus = purchasOrderFinanceItem.AllocatedAmount == null ?
                            FinanceAllocationStatus.NotAllocated :
                            (purchaseOrder.Price * purchaseOrder.Qty) == (purchasOrderFinanceItem.AllocatedAmount) ?
                            FinanceAllocationStatus.CompletelyAllocated : FinanceAllocationStatus.IncompleteAllocated,
                          IsArchived = purchaseOrder.IsArchived,
                          RowVersion = purchaseOrder.RowVersion,
                          MaxEstimateDateTime = cargoItem.MaxEstimateDateTime,
                          MinDeadlinePurchaseRequest = g.MinDeadlinePurchaseRequest,
                          PurchaseOrderStepDetailId = purchaseOrder.PurchaseOrderStepDetailId,
                          PurchaseOrderStepChangeTime = purchaseOrder.PurchaseOrderStepDetail.DateTime,
                          PurchaseOrderStepChangeUserFullName = purchaseOrder.PurchaseOrderStepDetail.User.Employee.FirstName + " " +
                            purchaseOrder.PurchaseOrderStepDetail.User.Employee.LastName,
                          PurchaseOrderStepId = purchaseOrder.PurchaseOrderStepDetail.PurchaseOrderStepId,
                          PurchaseOrderStepName = purchaseOrder.PurchaseOrderStepDetail.PurchaseOrderStep.Name,
                          PurchaseOrderStepDetailDescription = purchaseOrder.PurchaseOrderStepDetail.Description,
                          RiskLevelStatus = purchaseOrder.LatestRisk == null ? RiskLevelStatus.Low : purchaseOrder.LatestRisk.LatestRiskStatus.RiskParameter.RiskLevelStatus,
                          LatestRiskTitle = purchaseOrder.LatestRisk.Title,
                          LatestRiskCreateDateTime = purchaseOrder.LatestRisk.CreateDateTime
                        };
      return resultQuery;
    }
    public Expression<Func<PurchaseOrder, PurchaseOrderResult>> ToPurchaseOrderResult =
        purchaseOrder => new PurchaseOrderResult()
        {
          Id = purchaseOrder.Id,
          Code = purchaseOrder.Code,
          DateTime = purchaseOrder.DateTime,
          SupplierId = purchaseOrder.SupplierId,
          SupplierFullName = purchaseOrder.Supplier.Employee.FirstName + " " + purchaseOrder.Supplier.Employee.LastName,
          PurchaseOrderType = purchaseOrder.PurchaseOrderType,
          ProviderId = purchaseOrder.ProviderId,
          ProviderName = purchaseOrder.StuffProvider.Provider.Name,
          ProviderCode = purchaseOrder.StuffProvider.Provider.Code,
          StuffId = purchaseOrder.StuffId,
          StuffCode = purchaseOrder.Stuff.Code,
          StuffName = purchaseOrder.Stuff.Name,
          StuffGrossWeight = purchaseOrder.Stuff.GrossWeight,
          Price = purchaseOrder.Price,
          CurrencuyId = purchaseOrder.CurrencyId,
          CurrencyTitle = purchaseOrder.Currency.Title,
          CurrencySign = purchaseOrder.Currency.Sign,
          CurrencyDecimalDigitCount = purchaseOrder.Currency.DecimalDigitCount,
          Qty = purchaseOrder.Qty,
          UnitId = purchaseOrder.UnitId,
          UnitName = purchaseOrder.Unit.Name,
          ConversionRatio = purchaseOrder.Unit.ConversionRatio,
          DecimalDigitCount = purchaseOrder.Unit.DecimalDigitCount,
          Deadline = purchaseOrder.Deadline,
          Description = purchaseOrder.Description,
          CargoedQty = purchaseOrder.PurchaseOrderSummary.CargoedQty,
          QualityControlPassedQty = purchaseOrder.PurchaseOrderSummary.QualityControlPassedQty,
          QualityControlFailedQty = purchaseOrder.PurchaseOrderSummary.QualityControlFailedQty,
          ReceiptedQty = purchaseOrder.PurchaseOrderSummary.ReceiptedQty,
          RemainedQty = purchaseOrder.Qty - purchaseOrder.PurchaseOrderSummary.CargoedQty,
          StuffCategoryId = purchaseOrder.Stuff.StuffCategoryId,
          StuffCategoryName = purchaseOrder.Stuff.StuffCategory.Name,
          PurchaseOrderStatus = purchaseOrder.Status,
          FinancialTransacionBatchId = purchaseOrder.FinancialTransactionBatch.Id,
          EmployeeFullName = purchaseOrder.User.Employee.FirstName + " " + purchaseOrder.User.Employee.LastName,
          PurchaseOrderGroupCode = purchaseOrder.PurchaseOrderGroup.Code,
          PurchaseOrderGroupId = purchaseOrder.PurchaseOrderGroup.Id,
          RowVersion = purchaseOrder.RowVersion,
        };
    #endregion
    #region Search
    public IQueryable<PurchaseOrderResult> SearchPurchaseOrderResult(
        IQueryable<PurchaseOrderResult> query,
        IQueryable<PurchaseOrderDetail> purchaseOrderDetails,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText,
        DateTime? fromDateTime,
        DateTime? toDateTime,
        DateTime? fromDeadlineDateTime,
        DateTime? toDeadlineDateTime,
        int[] purchaseOrderIds,
        string purchaseOrderGroupCode,
        int? purchaseOrderGroupId,
        int? stuffCategoryId,
        int? planCodeId,
        FinanceAllocationStatus? financeAllocationStatus,
        RiskLevelStatus? riskLevelStatus)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where
                      item.Code.Contains(searchText) ||
                      item.StuffName.Contains(searchText) ||
                      item.LatestRiskTitle.Contains(searchText) ||
                      item.StuffCode.Contains(searchText) ||
                      item.Description.Contains(searchText) ||
                      item.CurrencyTitle.Contains(searchText) ||
                      item.ProviderCode.Contains(searchText) ||
                      item.ProviderName.Contains(searchText) ||
                      item.PurchaseOrderGroupCode.Contains(searchText) ||
                      item.UnitName.Contains(searchText)
                select item;
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);
      if (fromDeadlineDateTime != null)
        query = query.Where(i => i.Deadline >= fromDeadlineDateTime);
      if (toDeadlineDateTime != null)
        query = query.Where(i => i.Deadline >= toDeadlineDateTime);
      if (purchaseOrderIds != null)
        query = query.Where(i => purchaseOrderIds.Contains(i.Id));
      if (stuffCategoryId != null)
        query = query.Where(i => i.StuffCategoryId == stuffCategoryId);
      if (financeAllocationStatus != null)
        query = query.Where(i => i.FinanceAllocationStatus == financeAllocationStatus);
      if (riskLevelStatus != null)
        query = query.Where(i => i.RiskLevelStatus == riskLevelStatus);
      var hasPlanCode = advanceSearchItems.Where(m => m.FieldName == "PlanCode");
      if (hasPlanCode.Any())
      {
        advanceSearchItems.ForEach(r =>
        {
          if (r.FieldName == "PlanCode")
          {
            if (r.Value != null)
            {
              var getPurchaseOrderIds = (from q in purchaseOrderDetails
                                         where (q.PurchaseRequest.PlanCode.Code.Contains(r.Value.ToString()))
                                         select q.PurchaseOrderId).Distinct();
              query = from q in query
                      join purchaseOrderId in getPurchaseOrderIds
                              on q.Id equals purchaseOrderId
                      select q;
            }
          }
        });
        advanceSearchItems = advanceSearchItems.Where(m => m.FieldName != "PlanCode").ToArray();
      }
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      if (purchaseOrderGroupCode != null)
        query = query.Where(i => i.PurchaseOrderGroupCode == purchaseOrderGroupCode);
      if (purchaseOrderGroupId != null)
        query = query.Where(i => i.PurchaseOrderGroupId == purchaseOrderGroupId);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<PurchaseOrderResult> SortPurchaseOrderResult(IQueryable<PurchaseOrderResult> query, SearchInput<PurchaseOrderSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case PurchaseOrderSortType.Code:
          return query.OrderBy(i => i.Code, sortInput.SortOrder);
        case PurchaseOrderSortType.OrderInvoiceNum:
          return query.OrderBy(i => i.OrderInvoiceNum, sortInput.SortOrder);
        case PurchaseOrderSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sortInput.SortOrder);
        case PurchaseOrderSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sortInput.SortOrder);
        case PurchaseOrderSortType.Price:
          return query.OrderBy(i => i.Price, sortInput.SortOrder);
        case PurchaseOrderSortType.TotalPrice:
          return query.OrderBy(i => i.TotalPrice, sortInput.SortOrder);
        case PurchaseOrderSortType.CurrencyTitle:
          return query.OrderBy(i => i.CurrencyTitle, sortInput.SortOrder);
        case PurchaseOrderSortType.Deadline:
          return query.OrderBy(i => i.Deadline, sortInput.SortOrder);
        case PurchaseOrderSortType.DateTime:
          return query.OrderBy(i => i.DateTime, sortInput.SortOrder);
        case PurchaseOrderSortType.PurchaseOrderDateTime:
          return query.OrderBy(i => i.PurchaseOrderDateTime, sortInput.SortOrder);
        case PurchaseOrderSortType.Qty:
          return query.OrderBy(i => i.Qty, sortInput.SortOrder);
        case PurchaseOrderSortType.RemainedQty:
          return query.OrderBy(i => i.RemainedQty, sortInput.SortOrder);
        case PurchaseOrderSortType.PurchaseOrderStatus:
          return query.OrderBy(i => i.PurchaseOrderStatus, sortInput.SortOrder);
        case PurchaseOrderSortType.UnitName:
          return query.OrderBy(i => i.UnitName, sortInput.SortOrder);
        case PurchaseOrderSortType.StuffCategoryName:
          return query.OrderBy(i => i.StuffCategoryName, sortInput.SortOrder);
        case PurchaseOrderSortType.CargoedQty:
          return query.OrderBy(i => i.CargoedQty, sortInput.SortOrder);
        case PurchaseOrderSortType.RemainingAmount:
          return query.OrderBy(i => i.RemainingAmount, sortInput.SortOrder);
        case PurchaseOrderSortType.AllocatedAmount:
          return query.OrderBy(i => i.AllocatedAmount, sortInput.SortOrder);
        case PurchaseOrderSortType.EmployeeFullName:
          return query.OrderBy(i => i.EmployeeFullName, sortInput.SortOrder);
        case PurchaseOrderSortType.ProviderName:
          return query.OrderBy(i => i.ProviderName, sortInput.SortOrder);
        case PurchaseOrderSortType.ProviderCode:
          return query.OrderBy(i => i.ProviderCode, sortInput.SortOrder);
        case PurchaseOrderSortType.SupplierFullName:
          return query.OrderBy(i => i.SupplierFullName, sortInput.SortOrder);
        case PurchaseOrderSortType.PurchaseOrderType:
          return query.OrderBy(i => i.PurchaseOrderType, sortInput.SortOrder);
        case PurchaseOrderSortType.ReceiptedQty:
          return query.OrderBy(i => i.ReceiptedQty, sortInput.SortOrder);
        case PurchaseOrderSortType.QualityControlPassedQty:
          return query.OrderBy(i => i.QualityControlPassedQty, sortInput.SortOrder);
        case PurchaseOrderSortType.QualityControlFailedQty:
          return query.OrderBy(i => i.QualityControlFailedQty, sortInput.SortOrder);
        case PurchaseOrderSortType.PurchaseRequsetDescription:
          return query.OrderBy(i => i.Code);//.OrderBy(i => i.PurchaseRequestDescriptionArray, sortInput.SortOrder);  Not Supported !!!
        case PurchaseOrderSortType.LatestBaseEntityDocumentDescription:
          return query.OrderBy(i => i.LatestBaseEntityDocumentDescription, sortInput.SortOrder);
        case PurchaseOrderSortType.LatestBaseEntityDocumentDateTime:
          return query.OrderBy(i => i.LatestBaseEntityDocumentDateTime, sortInput.SortOrder);
        case PurchaseOrderSortType.PlanCode:
          return query.OrderBy(i => i.Code);//OrderBy(i => i.PlanCodeArray, sortInput.SortOrder);  Not Supported !!!
        case PurchaseOrderSortType.PriceConfirmationStatus:
          return query.OrderBy(i => i.PriceConfirmationStatus, sortInput.SortOrder);
        case PurchaseOrderSortType.PriceConfirmerFullName:
          return query.OrderBy(i => i.PriceConfirmerFullName, sortInput.SortOrder);
        case PurchaseOrderSortType.PriceConfirmDescription:
          return query.OrderBy(i => i.PriceConfirmDescription, sortInput.SortOrder);
        case PurchaseOrderSortType.IsArchived:
          return query.OrderBy(i => i.IsArchived, sortInput.SortOrder);
        case PurchaseOrderSortType.PurchaseOrderStepName:
          return query.OrderBy(i => i.PurchaseOrderStepName, sortInput.SortOrder);
        case PurchaseOrderSortType.PurchaseOrderStepDetailDescription:
          return query.OrderBy(i => i.PurchaseOrderStepDetailDescription, sortInput.SortOrder);
        case PurchaseOrderSortType.PurchaseOrderStepChangeTime:
          return query.OrderBy(i => i.PurchaseOrderStepChangeTime, sortInput.SortOrder);
        case PurchaseOrderSortType.FinanceAllocationStatus:
          return query.OrderBy(i => i.FinanceAllocationStatus, sortInput.SortOrder);
        case PurchaseOrderSortType.RiskLevelStatus:
          return query.OrderBy(i => i.RiskLevelStatus, sortInput.SortOrder);
        case PurchaseOrderSortType.PurchaseOrderGroupCode:
          return query.OrderBy(i => i.PurchaseOrderGroupCode, sortInput.SortOrder);
        case PurchaseOrderSortType.MinDeadlinePurchaseRequest:
          return query.OrderBy(i => i.MinDeadlinePurchaseRequest, sortInput.SortOrder);
        case PurchaseOrderSortType.MaxEstimateDateTime:
          return query.OrderBy(i => i.MaxEstimateDateTime, sortInput.SortOrder);
        case PurchaseOrderSortType.PurchaseOrderPreparingDate:
          return query.OrderBy(i => i.PurchaseOrderPreparingDateTime, sortInput.SortOrder);
        case PurchaseOrderSortType.StuffGrossWeight:
          return query.OrderBy(i => i.StuffGrossWeight, sortInput.SortOrder);
        case PurchaseOrderSortType.LatestRiskTitle:
          return query.OrderBy(i => i.LatestRiskTitle, sortInput.SortOrder);
        case PurchaseOrderSortType.LatestRiskCreateDateTime:
          return query.OrderBy(i => i.LatestRiskCreateDateTime, sortInput.SortOrder);
        case PurchaseOrderSortType.StuffType:
          return query.OrderBy(i => i.StuffType, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Remove
    public void RemovePurchaseOrder(int id, byte[] rowVersion)
    {

      var purchaseOrder = GetPurchaseOrder(id: id);
      if (purchaseOrder == null)
        throw new PurchaseOrderNotFoundException(id);
      if ((purchaseOrder.Status & PurchaseOrderStatus.Cargoing) > 0)
        throw new PurchaseOrderCanNotDeleteException(id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: null,
                    baseEntity: purchaseOrder,
                    rowVersion: rowVersion);
    }
    #endregion
    #region RemoveProcess
    public void RemovePurchaseOrderProcess(
        int id,
        byte[] rowVersion,
        TransactionBatch transactionBatch)
    {

      var purchaseOrder = GetPurchaseOrder(id: id);
      if (purchaseOrder == null)
        throw new PurchaseOrderNotFoundException(id);
      if (purchaseOrder.Status != PurchaseOrderStatus.NotAction)
        throw new PurchaseOrderCanNotDeleteException(id);
      #region RemovePurchaseOrder
      RemovePurchaseOrder(
          id: id,
          rowVersion: rowVersion);
      #endregion
      #region RemoveTransactionBatch
      if (purchaseOrder.TransactionBatch != null)
        App.Internals.WarehouseManagement.RemoveTransactionBatchProcess(
                      oldTransactionBathId: purchaseOrder.TransactionBatch.Id,
                      newTransactionBatchId: null);
      #endregion
      #region RemoveFinancialTransactions
      var financialTransactionBatch = App.Internals.Accounting.GetFinancialTransactionBatch(
      purchaseOrder.FinancialTransactionBatch.Id);
      var financialTransactions = App.Internals.Accounting.GetFinancialTransactions(
                selector: e => e,
                financialTransactionBatchId: financialTransactionBatch.Id); ; var newFinancialTransactionBatch = App.Internals.Accounting.AddFinancialTransactionBatch();
      ///حذف تراکنش های مالی
      foreach (var item in financialTransactions)
      {
        App.Internals.Accounting.DeleteFinancialTransaction(item.Id);
      }
      #endregion
      #region Remove PurchaseOrderDetails
      var purchaseOrderDetails = GetPurchaseOrderDetails(selector: e => e,
    purchaseOrderId: id);
      foreach (var purchaseOrderDetail in purchaseOrderDetails)
      {
        RemovePurchaseOrderDetailProcess(
                      transactionBatch: purchaseOrderDetail.TransactionBatch,
                      id: purchaseOrderDetail.Id,
                      rowVersion: purchaseOrderDetail.RowVersion);
      }
      #endregion
      #region Remove EstimatedPurchasePrice
      var estimatedPurchasePrices = purchaseOrder.EstimatedPurchasePrices.ToList();
      foreach (var item in estimatedPurchasePrices)
      {
        DeleteEstimatedPurchasePrice(estimatedPurchasePrice: item);
      }
      #endregion
    }
    public void UpdatePurchaseOrderPriceStatus(
        int purchaseOrderId,
        ConfirmationStatus status,
        string description,
        byte[] rowVersion)
    {

      var confirmModule = App.Internals.Confirmation;
      var confirmPurchaseOrders = confirmModule
                .GetBaseEntityConfirmations(x => x, confirmingEntityId: purchaseOrderId)


                .Where(x => !x.IsDelete)
                .ToList();
      confirmPurchaseOrders.ForEach(entity =>
            {
              confirmModule
                        .EditBaseEntityConfirmation(baseEntityConfirmation: entity, rowVersion: entity.RowVersion, isDelete: true);
            });
      confirmModule.AddBaseEntityConfirmation(
                baseEntityConfirmTypeId: lena.Models.StaticData.StaticBaseEntityConfirmTypes.PurchaseOrderPriceConfirmation.Id,
                confirmingEntityId: purchaseOrderId,
                status: status,
                confirmDescription: description
            );
    }
    #endregion
    #region ResetStatus
    public PurchaseOrder ResetPurchaseOrderStatus(int purchaseOrderId)
    {

      var purchaseOrder = GetPurchaseOrder(id: purchaseOrderId);
      return ResetPurchaseOrderStatus(purchaseOrder: purchaseOrder);
    }
    public PurchaseOrder ResetPurchaseOrderStatus(PurchaseOrder purchaseOrder)
    {

      #region ResetOrderItemSummary
      var purchaseOrderSummary = ResetPurchaseOrderSummaryByPurchaseOrderId(
    purchaseOrderId: purchaseOrder.Id);
      #endregion
      #region Define Status
      var status = PurchaseOrderStatus.None;
      if (purchaseOrderSummary.CargoedQty > 0)
      {
        if (purchaseOrderSummary.CargoedQty >= purchaseOrderSummary.PurchaseOrder.Qty)
          status = status | PurchaseOrderStatus.Cargoed;
        else
          status = status | PurchaseOrderStatus.Cargoing;
      }
      if (purchaseOrderSummary.ReceiptedQty > 0)
      {
        if (purchaseOrderSummary.ReceiptedQty >= purchaseOrderSummary.PurchaseOrder.Qty)
          status = status | PurchaseOrderStatus.Receipted;
        else
          status = status | PurchaseOrderStatus.Receipting;
      }
      if (purchaseOrderSummary.QualityControlPassedQty > 0)
      {
        if (purchaseOrderSummary.QualityControlPassedQty >= purchaseOrderSummary.PurchaseOrder.Qty)
          status = status | PurchaseOrderStatus.QualityControled;
        else
          status = status | PurchaseOrderStatus.QualityControling;
      }
      if (status == PurchaseOrderStatus.None)
        status = PurchaseOrderStatus.NotAction;
      #endregion
      #region Edit PurchaseOrder
      if (status != purchaseOrder.Status)
        EditPurchaseOrder(
                      purchaseOrder: purchaseOrder,
                      rowVersion: purchaseOrder.RowVersion,
                      status: status);
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
    baseEntityId: purchaseOrder.Id,
    scrumTaskType: ScrumTaskTypes.Shipping);
      #endregion
      #region FinalyTask
      if (purchaseOrder.Status == PurchaseOrderStatus.Cargoed)
      {
        if (projectWorkItem != null)
        {
          #region DoneTask
          App.Internals.ScrumManagement.DoneScrumTask(
    scrumTask: projectWorkItem,
    rowVersion: projectWorkItem.RowVersion);
          #endregion
        }
      }
      else
      {
        #region Add ShippingTrackingTask
        if (projectWorkItem == null)
        {
          projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                        baseEntityId: purchaseOrder.Id,
                        scrumTaskType: ScrumTaskTypes.Shipping);
          //check projectWork not null
          if (projectWorkItem != null)
          {
            App.Internals.ProjectManagement.AddProjectWorkItem(
                          projectWorkItem: null,
                          name: $"ثبت محموله سفارش خرید {purchaseOrder.Code} ",
                          description: "",
                          color: "",
                          departmentId: (int)Departments.Supplies,
                          estimatedTime: 10800,
                          isCommit: false,
                          scrumTaskTypeId: (int)ScrumTaskTypes.Shipping,
                          userId: null,
                          spentTime: 0,
                          remainedTime: 0,
                          scrumTaskStep: ScrumTaskStep.ToDo,
                          projectWorkId: projectWorkItem.ScrumBackLogId,
                          baseEntityId: purchaseOrder.Id);
          }
        }
        #endregion
      }
      #endregion
      return purchaseOrder;
    }
    #endregion
  }
}