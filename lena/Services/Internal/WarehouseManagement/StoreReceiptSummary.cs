using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.QualityControl.QualityControl;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Get ByStoreReceiptId
    public StoreReceiptSummary GetStoreReceiptSummaryByStoreReceiptId(int storeReceiptId) => GetStoreReceiptSummaryByStoreReceiptId(selector: e => e, storeReceiptId: storeReceiptId);
    public TResult GetStoreReceiptSummaryByStoreReceiptId<TResult>(
        Expression<Func<StoreReceiptSummary, TResult>> selector,
        int storeReceiptId)
    {

      var storeReceiptSummary = GetStoreReceiptSummaries(
                    selector: selector,
                    storeReceiptId: storeReceiptId)


                .FirstOrDefault();
      if (storeReceiptSummary == null)
        throw new StoreReceiptSummaryForStoreReceiptNotFoundException(storeReceiptId: storeReceiptId);
      return storeReceiptSummary;
    }
    #endregion
    #region Get
    public StoreReceiptSummary GetStoreReceiptSummary(int id) => GetStoreReceiptSummary(selector: e => e, id: id);
    public TResult GetStoreReceiptSummary<TResult>(
        Expression<Func<StoreReceiptSummary, TResult>> selector,
        int id)
    {

      var storeReceiptSummary = GetStoreReceiptSummaries(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (storeReceiptSummary == null)
        throw new StoreReceiptSummaryNotFoundException(id: id);
      return storeReceiptSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStoreReceiptSummaries<TResult>(
            Expression<Func<StoreReceiptSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> qualityControlPassedQty = null,
            TValue<double> qualityControlFailedQty = null,
            TValue<double> qualityControlConsumedQty = null,
            TValue<int> storeReceiptId = null)
    {

      var query = repository.GetQuery<StoreReceiptSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (qualityControlPassedQty != null)
        query = query.Where(x => x.QualityControlPassedQty == qualityControlPassedQty);
      if (qualityControlFailedQty != null)
        query = query.Where(x => x.QualityControlFailedQty == qualityControlFailedQty);
      if (qualityControlConsumedQty != null)
        query = query.Where(x => x.QualityControlConsumedQty == qualityControlConsumedQty);
      if (storeReceiptId != null)
        query = query.Where(x => x.StoreReceipt.Id == storeReceiptId);

      return query.Select(selector);
    }
    #endregion
    #region Add

    public StoreReceiptSummary AddStoreReceiptSummary(
        double qualityControlPassedQty,
        double qualityControlFailedQty,
        double qualityControlConsumedQty,
        double payedAmount,
        int storeReceiptId)
    {

      var storeReceiptSummary = repository.Create<StoreReceiptSummary>();
      storeReceiptSummary.QualityControlPassedQty = qualityControlPassedQty;
      storeReceiptSummary.QualityControlFailedQty = qualityControlFailedQty;
      storeReceiptSummary.QualityControlConsumedQty = qualityControlConsumedQty;
      storeReceiptSummary.ReceiptQualityControlPassedQty = qualityControlPassedQty;
      storeReceiptSummary.ReceiptQualityControlFailedQty = qualityControlFailedQty;
      storeReceiptSummary.ReceiptQualityControlConsumedQty = qualityControlConsumedQty;
      storeReceiptSummary.PayedAmount = payedAmount;
      storeReceiptSummary.StoreReceipt = GetStoreReceipt(id: storeReceiptId);
      repository.Add(storeReceiptSummary);
      return storeReceiptSummary;
    }

    #endregion
    #region Edit
    public StoreReceiptSummary EditStoreReceiptSummary(
        int id,
        byte[] rowVersion,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null,
        TValue<double> qualityControlConsumedQty = null,
        TValue<double> payedAmount = null)
    {

      var storeReceiptSummary = GetStoreReceiptSummary(id: id);
      return EditStoreReceiptSummary(
                    storeReceiptSummary: storeReceiptSummary,
                    rowVersion: rowVersion,
                    qualityControlPassedQty: qualityControlPassedQty,
                    qualityControlFailedQty: qualityControlFailedQty,
                    qualityControlConsumedQty: qualityControlConsumedQty,
                    payedAmount: payedAmount);
    }

    public StoreReceiptSummary EditStoreReceiptSummary(
                StoreReceiptSummary storeReceiptSummary,
                byte[] rowVersion,
                TValue<double> qualityControlPassedQty = null,
                TValue<double> qualityControlFailedQty = null,
                TValue<double> qualityControlConsumedQty = null,
                TValue<double> receiptQualityControlPassedQty = null,
                TValue<double> receiptQualityControlFailedQty = null,
                TValue<double> receiptQualityControlConsumedQty = null,
                TValue<double> payedAmount = null)

    {

      if (qualityControlPassedQty != null)
        storeReceiptSummary.QualityControlPassedQty = qualityControlPassedQty;
      if (qualityControlFailedQty != null)
        storeReceiptSummary.QualityControlFailedQty = qualityControlFailedQty;
      if (qualityControlConsumedQty != null)
        storeReceiptSummary.QualityControlConsumedQty = qualityControlConsumedQty;
      if (payedAmount != null)
        storeReceiptSummary.PayedAmount = payedAmount;

      var receipt = GetStoreReceiptSummaries(selector: e => e.StoreReceipt.Receipt, id: storeReceiptSummary.Id)


                .FirstOrDefault();
      if (receipt != null)
      {
        if (receiptQualityControlPassedQty != null)
          storeReceiptSummary.ReceiptQualityControlPassedQty = receiptQualityControlPassedQty;
        else if (storeReceiptSummary.ReceiptQualityControlPassedQty == 0 && qualityControlPassedQty != 0)
          storeReceiptSummary.ReceiptQualityControlPassedQty = qualityControlPassedQty;

        if (receiptQualityControlFailedQty != null)
          storeReceiptSummary.ReceiptQualityControlFailedQty = receiptQualityControlFailedQty;
        else if (storeReceiptSummary.ReceiptQualityControlFailedQty != 0 && qualityControlPassedQty != 0)
        {
          storeReceiptSummary.ReceiptQualityControlFailedQty = 0;
          storeReceiptSummary.ReceiptQualityControlPassedQty = qualityControlPassedQty;
        }
        else if (storeReceiptSummary.ReceiptQualityControlFailedQty == 0 && qualityControlFailedQty != 0)
          storeReceiptSummary.ReceiptQualityControlFailedQty = qualityControlFailedQty;

        if (receiptQualityControlConsumedQty != null)
          storeReceiptSummary.ReceiptQualityControlConsumedQty = receiptQualityControlConsumedQty;
        else if (storeReceiptSummary.ReceiptQualityControlConsumedQty == 0 && qualityControlConsumedQty != 0)
          storeReceiptSummary.ReceiptQualityControlConsumedQty = qualityControlConsumedQty;
      }
      else if (receipt == null || receipt.Status.HasFlag(ReceiptStatus.EternalReceipt) == false)
      {
        storeReceiptSummary.ReceiptQualityControlPassedQty = storeReceiptSummary.QualityControlPassedQty;
        storeReceiptSummary.ReceiptQualityControlFailedQty = storeReceiptSummary.QualityControlFailedQty;
        storeReceiptSummary.ReceiptQualityControlConsumedQty = storeReceiptSummary.QualityControlConsumedQty;
      }
      repository.Update(rowVersion: rowVersion, entity: storeReceiptSummary);

      return storeReceiptSummary;
    }

    #endregion
    #region Delete
    public void DeleteStoreReceiptSummary(int id)
    {

      var storeReceiptSummary = GetStoreReceiptSummary(id: id);
      repository.Delete(storeReceiptSummary);
    }
    #endregion
    #region Reset
    public StoreReceiptSummary ResetStoreReceiptSummaryByStoreReceiptId(int storeReceiptId)
    {

      var storeReceiptSummary = GetStoreReceiptSummaryByStoreReceiptId(storeReceiptId: storeReceiptId); ; return ResetStoreReceiptSummary(storeReceiptSummary: storeReceiptSummary);

    }
    public StoreReceiptSummary ResetStoreReceiptSummary(int id)
    {

      var storeReceiptSummary = GetStoreReceiptSummary(id: id); ; return ResetStoreReceiptSummary(storeReceiptSummary: storeReceiptSummary);

    }
    public StoreReceiptSummary ResetStoreReceiptSummary(StoreReceiptSummary storeReceiptSummary)
    {


      var storeReceiptInfo = GetStoreReceiptSummary(
                selector: e => new
                {
                  Id = e.StoreReceipt.Id,
                  Code = e.StoreReceipt.Code,
                  StuffId = e.StoreReceipt.StuffId,
                  SerialProfileCode = (int?)e.StoreReceipt.StoreReceiptSerialProfile.Code,
                  //Serials = e.StoreReceipt.StoreReceiptSerialProfile.StuffSerials,
                  UnitId = e.StoreReceipt.UnitId,
                  UnitConversionRatio = e.StoreReceipt.Unit.ConversionRatio,
                  StoreReceiptType = e.StoreReceipt.StoreReceiptType,
                  RowVersion = e.StoreReceipt.RowVersion,
                  ReceiptId = e.StoreReceipt.ReceiptId,
                  DateTime = e.StoreReceipt.DateTime,
                  ReceiptStatus = e.StoreReceipt.ReceiptId == null ? ReceiptStatus.NoReceipt : e.StoreReceipt.Receipt.Status,
                  ReceipRowVersion = e.StoreReceipt.Receipt.RowVersion
                },
                id: storeReceiptSummary.Id);

      IQueryable<QualityControlResetReceiptResult> qualityControls = null;

      if (storeReceiptInfo.StoreReceiptType == StoreReceiptType.NewShopping)
      {
        //Load NewShopping Qc
        //Load Custom Qc

        var serials = GetNewShoppingSerials(storeReceiptInfo.Id)


        .Select(i => i.Serial)
        .ToArray();

        //لیست کنترل کیفی های دستی 
        qualityControls = App.Internals.QualityControl.GetQualityControls(
        selector: e => new QualityControlResetReceiptResult()
        {
          StoreReceiptId = e.Id,
          StuffId = e.StuffId,
          Qty = e.Qty * e.Unit.ConversionRatio,
          DateTime = e.DateTime,
          ReturnOfSaleConversionRatio = e.Unit.ConversionRatio,
          PayedAmount = e.PayRequest == null ? 0 : e.PayRequest.PayedAmount
        },
            isDelete: false,
            storeReceiptCode: storeReceiptInfo.Code,
            serials: serials,
            qualityControlType: QualityControlType.CustomQualityControl

           );

        //لیست کنترل کیفی خرید جدید
        var returnOfSaleQcs = qualityControls = App.Internals.QualityControl.GetQualityControls(
            selector: e => new QualityControlResetReceiptResult()
            {
              StoreReceiptId = e.Id,
              StuffId = e.StuffId,
              Qty = e.Qty * e.Unit.ConversionRatio,
              DateTime = e.DateTime,
              ReturnOfSaleConversionRatio = e.Unit.ConversionRatio,
              PayedAmount = e.PayRequest == null ? 0 : e.PayRequest.PayedAmount
            },
                isDelete: false,
                storeReceiptCode: storeReceiptInfo.Code
               );

        qualityControls = qualityControls.Union(returnOfSaleQcs);

      }
      else if (storeReceiptInfo.StoreReceiptType == StoreReceiptType.ReturnOfSale)
      {
        //Load Custom Qc (DateTime >= ReturnOfSale Stuff Enter Time)
        //load ReturnOfsale Qc

        var serials = GetReturnOfSaleSerials(storeReceiptInfo.Id)


        .Select(i => i.Serial)
        .ToArray();

        //لیست کنترل کیفی های دستی که تاریخ انجام آن ها بعد از تاریخ برگشت از فروش می باشد
        qualityControls = App.Internals.QualityControl.GetQualityControls(
        selector: e => new QualityControlResetReceiptResult()
        {
          StoreReceiptId = e.Id,
          StuffId = e.StuffId,
          Qty = e.Qty * e.Unit.ConversionRatio,
          DateTime = e.DateTime,
          ReturnOfSaleConversionRatio = e.Unit.ConversionRatio,
          PayedAmount = e.PayRequest == null ? 0 : e.PayRequest.PayedAmount
        },
            isDelete: false,
            storeReceiptCode: storeReceiptInfo.Code,
            serials: serials,
            qualityControlType: QualityControlType.CustomQualityControl

           )


        .Where(i => i.DateTime >= storeReceiptInfo.DateTime);

        //لیست کنترل کیفی برگشت از فروش
        var returnOfSaleQcs = qualityControls = App.Internals.QualityControl.GetQualityControls(
            selector: e => new QualityControlResetReceiptResult()
            {
              StoreReceiptId = e.Id,
              StuffId = e.StuffId,
              Qty = e.Qty * e.Unit.ConversionRatio,
              DateTime = e.DateTime,
              ReturnOfSaleConversionRatio = e.Unit.ConversionRatio,
              PayedAmount = e.PayRequest == null ? 0 : e.PayRequest.PayedAmount
            },
                isDelete: false,
                storeReceiptCode: storeReceiptInfo.Code
               );

        qualityControls = qualityControls.Union(returnOfSaleQcs);


      }
      else
        throw new ResetStoreReceiptTypeNotSupportedException(storeReceiptId: storeReceiptInfo.Id, type: storeReceiptInfo.StoreReceiptType);

      //qualityControls = App.Internals.QualityControl.GetQualityControls(
      //     selector: e => new QualityControlResetReceiptResult()
      //     {
      //         StoreReceiptId = e.Id,
      //         StuffId = e.StuffId,
      //         Qty = e.Qty * e.Unit.ConversionRatio,
      //         DateTime = e.DateTime,
      //         ReturnOfSaleConversionRatio = e.Unit.ConversionRatio,
      //         PayedAmount = e.PayRequest == null ? 0 : e.PayRequest.PayedAmount
      //     },
      //         isDelete: false,
      //         //serialProfileCode: storeReceiptInfo.SerialProfileCode
      //         storeReceiptCode: storeReceiptInfo.Code,
      //         getAllQCTypes: true
      //        //? (int?)storeReceiptInfo.Id
      //        //: null
      //        )
      // 
      //;
      //// .Where(i => i.DateTime >= storeReceiptInfo.DateTime);




      var getqualityControlItems = App.Internals.QualityControl.GetQualityControlItems(
          selector: e => new
          {
            e.Id,
            e.QualityControlId,
            e.StuffId,
            e.StuffSerialCode,
            Qty = e.Qty * e.Unit.ConversionRatio,
            DateTime = e.DateTime,
            ConversionRatio = e.Unit.ConversionRatio,
            QualityControlDateTime = e.QualityControl.DateTime,
            PayedAmount = e.QualityControl.PayRequest == null ? 0 : e.QualityControl.PayRequest.PayedAmount
          },
          isDelete: false//,
                         //storeReceiptId: storeReceiptInfo.Id
              );


      var qualityControlItems = (from qci in getqualityControlItems
                                 join qc in qualityControls on qci.QualityControlId equals qc.StoreReceiptId
                                 select new
                                 {
                                   Id = qci.Id,
                                   qci.StuffId,
                                   qci.StuffSerialCode,
                                   Qty = qci.Qty * qci.ConversionRatio,
                                   DateTime = qci.DateTime,
                                   ReturnOfSaleConversionRatio = qci.ConversionRatio,
                                   QualityControlDateTime = qc.DateTime,
                                   PayedAmount = qc.PayedAmount
                                 });




      var qualityControlItemsGroupBy = (from item in qualityControlItems
                                        group item by new { item.StuffSerialCode }
                into gItems
                                        select new
                                        {
                                          Id = gItems.Select(x => x.Id).FirstOrDefault(),
                                          StuffId = gItems.Select(x => x.StuffId).FirstOrDefault(),
                                          StuffSerialCode = gItems.Select(x => x.StuffSerialCode).FirstOrDefault(),
                                          Qty = gItems.Max(x => x.Qty),
                                          QualityControlDateTime = gItems.Select(x => x.QualityControlDateTime).FirstOrDefault(),
                                          SumOfPayedAmount = gItems.Sum(x => x.PayedAmount)
                                        }).ToList();


      var qualityControlConfirmationItems = App.Internals.QualityControl.GetQualityControlConfirmationItems(
                        selector: e => new
                        {
                          e.Id,
                          DateTime = e.QualityControlItem.QualityControl.ConfirmationDateTime,
                          RemainedQty = e.RemainedQty * e.Unit.ConversionRatio,
                          ConsumeQty = e.ConsumeQty * e.Unit.ConversionRatio,
                          QualityControlItemId = e.QualityControlItem.Id,
                          Status = e.QualityControlConfirmation.Status
                        },
                      isDelete: false,
                       stuffId: storeReceiptInfo.StuffId,
                       serialProfileCode: storeReceiptInfo.SerialProfileCode
                      );

      var qualityControlConfirmationItemsGroup = (from qcci in qualityControlConfirmationItems
                                                  join qci in qualityControlItems on qcci.QualityControlItemId equals qci.Id
                                                  select new
                                                  {
                                                    qcci.Id,
                                                    qcci.DateTime,
                                                    qcci.RemainedQty,
                                                    qcci.ConsumeQty,
                                                    QualityControlItemId = qci.Id,
                                                    Status = qcci.Status,
                                                    qci.StuffSerialCode,
                                                    qci.StuffId,
                                                    qci.ReturnOfSaleConversionRatio,
                                                  });

      var conditionalQualityControlItems = App.Internals.QualityControl.GetConditionalQualityControlItems(
                      selector: e => new
                      {
                        e.Id,
                        e.DateTime,
                        e.Qty,
                        Status = e.ConditionalQualityControl.Status,
                        ResponseConditionalConfirmationDate = e.ConditionalQualityControl.ResponseConditionalConfirmationDate,
                        QualityControlConfirmationItemId = e.QualityControlConfirmationItemId
                      },
                      isDelete: false);

      var conditionalQualityControlItemsGroup = (from cqcci in conditionalQualityControlItems
                                                 join qcci in qualityControlConfirmationItemsGroup on cqcci.QualityControlConfirmationItemId equals qcci.Id
                                                 group new { qcci, cqcci } by new { cqcci.Status, cqcci.ResponseConditionalConfirmationDate } into qcciItam
                                                 select new
                                                 {
                                                   Qty = qcciItam.Sum(x => x.cqcci.Qty / x.qcci.ReturnOfSaleConversionRatio),
                                                   Status = qcciItam.Key.Status,
                                                   ResponseConditionalConfirmationDate = qcciItam.Key.ResponseConditionalConfirmationDate,
                                                 });


      #region ConditionalLastQty

      var ConditionalLastQty = 0d;
      var ConditionalLastQtyQuery = conditionalQualityControlItemsGroup
                .Where(i => i.Status == ConditionalQualityControlStatus.Accepted)
                .OrderByDescending(i => i.ResponseConditionalConfirmationDate);
      ConditionalLastQty = ConditionalLastQtyQuery.Any() ? ConditionalLastQtyQuery.FirstOrDefault().Qty / storeReceiptInfo.UnitConversionRatio : 0;
      #endregion


      #region LastQualityControlStatus
      var maxqualityControlStatusQuery = qualityControlConfirmationItemsGroup.Where(i => i.Status != QualityControlStatus.NotAction).OrderByDescending(i => i.DateTime);
      QualityControlStatus maxqualityControlStatus = QualityControlStatus.NotAction;
      if (maxqualityControlStatusQuery.Any())
      {
        maxqualityControlStatus = maxqualityControlStatusQuery.FirstOrDefault().Status;
      }

      #endregion

      #region AcceptedQty
      var receiptAccepted = 0d;
      var acceptedQuery = qualityControlConfirmationItemsGroup.Where(i => i.Status == QualityControlStatus.Accepted);
      receiptAccepted = acceptedQuery.Any() ? acceptedQuery.Sum(i => i.RemainedQty) / storeReceiptInfo.UnitConversionRatio : 0;
      #endregion


      #region FailedQty
      var receiptFailed = 0d;
      var failedQuery = qualityControlConfirmationItemsGroup.Where(i => i.Status == QualityControlStatus.Rejected);
      receiptFailed = failedQuery.Any() ? failedQuery.Sum(i => i.RemainedQty) / storeReceiptInfo.UnitConversionRatio : 0;
      #endregion


      #region ConditionalQty
      var receiptConditional = 0d;
      var conditionalQuery = conditionalQualityControlItemsGroup.Where(i => i.Status == ConditionalQualityControlStatus.Accepted);
      receiptConditional = conditionalQuery.Any() ? conditionalQuery.Sum(i => i.Qty) / storeReceiptInfo.UnitConversionRatio : 0;
      #endregion                                 }).Sum(i => i.conditionalQty) / returnOfSaleSummary.ReturnOfSale.Unit.ConversionRatio;


      #region CounsumedQty
      var receiptConsumed = 0d;
      receiptConsumed = qualityControlConfirmationItemsGroup.Any() ? qualityControlConfirmationItemsGroup.Sum(i => i.ConsumeQty) / storeReceiptInfo.UnitConversionRatio : 0;
      #endregion

      var qualityControlPassedQty = 0d;
      var qualityControlFailedQty = 0d;
      var receiptQualityControlPassedQty = 0d;
      var receiptQualityControlFailedQty = 0d;
      var receiptQualityControlConsumedQty = 0d;

      var failedRemained = receiptFailed - receiptConditional;
      if (failedRemained < 0)
        failedRemained = 0;
      //else if (failedRemained == 0)
      //{
      //    if (receiptConditional > 0)
      //        failedRemained = ConditionalLastQty;
      //}

      var qty = qualityControls.Max(i => (double?)i.Qty) ?? 0; // qualityControlItems.Max(i => i.Qty);
      qualityControlPassedQty = receiptAccepted > qty ? qty : receiptAccepted;

      //qualityControlPassedQty = ((receiptAccepted > qty ? qty : receiptAccepted) + (failedRemained)) > qty
      //                     ? qty
      //                     : ((receiptAccepted > qty ? qty : receiptAccepted) + (failedRemained));

      qualityControlFailedQty = failedRemained;

      //if (maxqualityControlStatus == QualityControlStatus.Rejected && receiptConditional > 0)
      //{
      //    qualityControlFailedQty = qty + (qualityControlFailedQty - qualityControlPassedQty);
      //    qualityControlPassedQty = qty - qualityControlFailedQty - receiptConsumed;
      //}
      if (receiptConditional > 0)
      {
        qualityControlPassedQty = qty - failedRemained - receiptConsumed;
        qualityControlFailedQty = failedRemained;
      }


      if (storeReceiptInfo.ReceiptId == null || storeReceiptInfo.ReceiptStatus != ReceiptStatus.EternalReceipt)
      {
        receiptQualityControlPassedQty = qualityControlPassedQty;
        receiptQualityControlFailedQty = qualityControlFailedQty;
        receiptQualityControlConsumedQty = receiptConsumed;
      }
      if (storeReceiptInfo.ReceiptId != null)
      {
        if (receiptQualityControlPassedQty == 0)
          receiptQualityControlPassedQty = qualityControlPassedQty;
        if (receiptQualityControlFailedQty == 0)
          receiptQualityControlFailedQty = qualityControlFailedQty;
      }

      //returnOfSaleSummary = EditReturnOfSaleSummary(
      //     returnOfSaleSummary: returnOfSaleSummary,
      //     rowVersion: returnOfSaleSummary.RowVersion,
      //     qualityControlPassedQty: qualityControlPassedQty,
      //     qualityControlFailedQty: qualityControlFailedQty,
      //     qualityControlConsumedQty: receiptConsumed,
      //     receiptQualityControlPassedQty: receiptQualityControlPassedQty,
      //     receiptQualityControlFailedQty: receiptQualityControlFailedQty,
      //     receiptQualityControlConsumedQty: receiptQualityControlConsumedQty
      //     )
      // 
      //;

      //return returnOfSaleSummary;






      #region Edit StoreReceiptSummary

      storeReceiptSummary = EditStoreReceiptSummary(
              storeReceiptSummary: storeReceiptSummary,
              rowVersion: storeReceiptSummary.RowVersion,
              qualityControlPassedQty: qualityControlPassedQty,
              qualityControlFailedQty: qualityControlFailedQty,
              qualityControlConsumedQty: receiptConsumed,
              payedAmount: qualityControlItemsGroupBy.Sum(i => i.SumOfPayedAmount),
              receiptQualityControlPassedQty: receiptQualityControlPassedQty,
              receiptQualityControlFailedQty: receiptQualityControlFailedQty,
              receiptQualityControlConsumedQty: receiptQualityControlConsumedQty);

      #endregion
      if (storeReceiptSummary.StoreReceipt.StoreReceiptType == StoreReceiptType.NewShopping)
      {
        #region Reset NewShoppingDetail

        #region GetNewShoppingDetails

        var newShoppingDetails = GetNewShoppingDetails(
            selector: e =>
                    new
                    {
                      e.Id,
                      e.RowVersion,
                      e.Qty,
                      e.UnitId,
                      e.Unit.ConversionRatio
                    },
                newShoppingId: storeReceiptSummary.StoreReceipt.Id,
                isDelete: false);

        #endregion

        #region GetStoreReceiptSummary values

        var tempQualityControlValues = GetStoreReceiptSummary(
                selector: e => new
                {
                  qualityControlPassedQty = e.QualityControlPassedQty,
                  qualityControlFailedQty = e.QualityControlFailedQty,
                  qualityControlConsumedQty = e.QualityControlConsumedQty,
                  UnitId = e.StoreReceipt.UnitId,
                  UnitConversionRatio = e.StoreReceipt.Unit.ConversionRatio

                },
                id: storeReceiptSummary.Id);

        var tempQualityControlPassedQty = tempQualityControlValues.qualityControlPassedQty *
                                                tempQualityControlValues.UnitConversionRatio;
        var tempQualityControlFailedQty = tempQualityControlValues.qualityControlFailedQty *
                                                tempQualityControlValues.UnitConversionRatio;
        var tempQualityControlConsumedQty = tempQualityControlValues.qualityControlConsumedQty *
                                                  tempQualityControlValues.UnitConversionRatio;

        #endregion

        foreach (var newShoppingDetail in newShoppingDetails)
        {
          #region Calculate NewShoppingValues

          var detailQty = newShoppingDetail.Qty * newShoppingDetail.ConversionRatio;
          var detailPassedQty = Math.Min(tempQualityControlPassedQty, detailQty);

          detailQty -= detailPassedQty;
          tempQualityControlPassedQty -= detailPassedQty;

          var detailFailedQty = Math.Min(tempQualityControlFailedQty, detailQty);
          detailQty -= detailFailedQty;
          tempQualityControlFailedQty -= detailFailedQty;

          var detailConsumedQty = Math.Min(tempQualityControlConsumedQty, detailQty);
          detailQty -= detailConsumedQty;
          tempQualityControlConsumedQty -= detailConsumedQty;

          #endregion

          #region GetNewShoppingDetailSummaryByNewShoppingDetailId

          var newShoppingDetailSummary =
              GetNewShoppingDetailSummaryByNewShoppingDetailId(newShoppingDetail.Id);

          #endregion

          #region EditNewShoppingDetailSummary

          EditNewShoppingDetailSummary(
                  newShoppingDetailSummary: newShoppingDetailSummary,
                  rowVersion: newShoppingDetailSummary.RowVersion,
                  qualityControlPassedQty: detailPassedQty / newShoppingDetail.ConversionRatio,
                  qualityControlFailedQty: detailFailedQty / newShoppingDetail.ConversionRatio,
                  qualityControlConsumedQty: detailConsumedQty / newShoppingDetail.ConversionRatio);

          #endregion

          //#region Get CargoItemDetail
          //var cargoItemDetail = GetNewShoppingDetailSummary(
          //        selector: e => e.NewShoppingDetail.LadingItemDetail.CargoItemDetail,
          //        id: newShoppingDetailSummary.Id)
          //    
          //;
          //#endregion

          //#region Reset CargoItemDetailStatus
          //App.Internals.Supplies.ResetCargoItemDetailStatus(cargoItemDetail: cargoItemDetail)
          //    
          //;
          //#endregion

          #region Get LadingItemDetail

          var ladingItemDetail = GetNewShoppingDetailSummary(
                  selector: e => e.NewShoppingDetail.LadingItemDetail,
                  id: newShoppingDetailSummary.Id);

          #endregion

          #region Reset LadingItemDetailStatus
          App.Internals.Supplies.ResetLadingItemDetailStatus(ladingItemDetail: ladingItemDetail);

          #endregion
        }

        #endregion
      }

      if (storeReceiptInfo.StoreReceiptType == StoreReceiptType.ReturnOfSale)
      {
        #region Reset ReturnOfSaleSummary

        var returnOfSales = GetReturnOfSales(
                selector: e => e,
                isDelete: false,
                returnStoreReceiptId: storeReceiptInfo.Id)


            .ToList();

        foreach (var returnOfSale in returnOfSales)
        {
          var item = returnOfSale.ReturnOfSaleSummary;
          if (item == null)
          {
            #region AddReturnOfSaleSummary

            item = AddReturnOfSaleSummary(
                    qualityControlPassedQty: 0,
                    qualityControlFailedQty: 0,
                    qualityControlConsumedQty: 0,
                    returnOfSaleId: returnOfSale.Id);

            #endregion
          }
          ResetReturnOfSaleSummary(returnOfSaleSummary: item);
        }


        #endregion
      }

      return storeReceiptSummary;
    }

    #endregion

  }
}
