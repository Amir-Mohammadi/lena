using System;
using System.Collections.Generic;
//using System.Data.Entity.SqlServer;
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

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {

    #region Get ByReturnOfSaleId
    public ReturnOfSaleSummary GetReturnOfSaleSummaryByReturnOfSaleId(int returnOfSaleId) => GetReturnOfSaleSummaryByReturnOfSaleId(selector: e => e, returnOfSaleId: returnOfSaleId);
    public TResult GetReturnOfSaleSummaryByReturnOfSaleId<TResult>(
        Expression<Func<ReturnOfSaleSummary, TResult>> selector,
        int returnOfSaleId)
    {

      var returnOfSaleSummary = GetReturnOfSaleSummaries(
                    selector: selector,
                    returnOfSaleId: returnOfSaleId)


                .FirstOrDefault();
      if (returnOfSaleSummary == null)
        throw new ReturnOfSaleSummaryForReturnOfSaleNotFoundException(returnOfSaleId: returnOfSaleId);
      return returnOfSaleSummary;
    }
    #endregion
    #region Get
    public ReturnOfSaleSummary GetReturnOfSaleSummary(int id) => GetReturnOfSaleSummary(selector: e => e, id: id);
    public TResult GetReturnOfSaleSummary<TResult>(
        Expression<Func<ReturnOfSaleSummary, TResult>> selector,
        int id)
    {

      var returnOfSaleSummary = GetReturnOfSaleSummaries(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (returnOfSaleSummary == null)
        throw new ReturnOfSaleSummaryNotFoundException(id: id);
      return returnOfSaleSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetReturnOfSaleSummaries<TResult>(
            Expression<Func<ReturnOfSaleSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> qualityControlPassedQty = null,
            TValue<double> qualityControlFailedQty = null,
            TValue<double> qualityControlConsumedQty = null,
            TValue<int> returnOfSaleId = null)
    {

      var query = repository.GetQuery<ReturnOfSaleSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (qualityControlPassedQty != null)
        query = query.Where(x => x.QualityControlPassedQty == qualityControlPassedQty);
      if (qualityControlFailedQty != null)
        query = query.Where(x => x.QualityControlFailedQty == qualityControlFailedQty);
      if (qualityControlConsumedQty != null)
        query = query.Where(x => x.QualityControlConsumedQty == qualityControlConsumedQty);
      if (returnOfSaleId != null)
        query = query.Where(x => x.ReturnOfSale.Id == returnOfSaleId);

      return query.Select(selector);
    }
    #endregion
    #region Add

    public ReturnOfSaleSummary AddReturnOfSaleSummary(
        double qualityControlPassedQty,
        double qualityControlFailedQty,
        double qualityControlConsumedQty,
        int returnOfSaleId)
    {

      var returnOfSaleSummary = repository.Create<ReturnOfSaleSummary>();
      returnOfSaleSummary.QualityControlPassedQty = qualityControlPassedQty;
      returnOfSaleSummary.QualityControlFailedQty = qualityControlFailedQty;
      returnOfSaleSummary.QualityControlConsumedQty = qualityControlConsumedQty;
      returnOfSaleSummary.ReceiptQualityControlPassedQty = qualityControlPassedQty;
      returnOfSaleSummary.ReceiptQualityControlFailedQty = qualityControlFailedQty;
      returnOfSaleSummary.ReceiptQualityControlConsumedQty = qualityControlConsumedQty;
      returnOfSaleSummary.ReturnOfSale = GetReturnOfSale(id: returnOfSaleId);
      repository.Add(returnOfSaleSummary);
      return returnOfSaleSummary;
    }

    #endregion
    #region Edit
    public ReturnOfSaleSummary EditReturnOfSaleSummary(
        int id,
        byte[] rowVersion,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null,
        TValue<double> qualityControlConsumedQty = null)
    {

      var returnOfSaleSummary = GetReturnOfSaleSummary(id: id);
      return EditReturnOfSaleSummary(
                    returnOfSaleSummary: returnOfSaleSummary,
                    rowVersion: rowVersion,
                    qualityControlPassedQty: qualityControlPassedQty,
                    qualityControlFailedQty: qualityControlFailedQty,
                    qualityControlConsumedQty: qualityControlConsumedQty);
    }

    public ReturnOfSaleSummary EditReturnOfSaleSummary(
                ReturnOfSaleSummary returnOfSaleSummary,
                byte[] rowVersion,
                TValue<double> qualityControlPassedQty = null,
                TValue<double> qualityControlFailedQty = null,
                TValue<double> qualityControlConsumedQty = null,
                TValue<double> receiptQualityControlPassedQty = null,
                TValue<double> receiptQualityControlFailedQty = null,
                TValue<double> receiptQualityControlConsumedQty = null)

    {

      if (qualityControlPassedQty != null)
        returnOfSaleSummary.QualityControlPassedQty = qualityControlPassedQty;
      if (qualityControlFailedQty != null)
        returnOfSaleSummary.QualityControlFailedQty = qualityControlFailedQty;
      if (qualityControlConsumedQty != null)
        returnOfSaleSummary.QualityControlConsumedQty = qualityControlConsumedQty;

      var receipt = GetReturnOfSaleSummaries(selector: e => e.ReturnOfSale.ReturnStoreReceipt.Receipt, id: returnOfSaleSummary.Id)


                .FirstOrDefault();
      if (receipt != null)
      {
        if (receiptQualityControlPassedQty != null)
          returnOfSaleSummary.ReceiptQualityControlPassedQty = receiptQualityControlPassedQty;
        else if (returnOfSaleSummary.ReceiptQualityControlPassedQty == 0 && qualityControlPassedQty != 0)
          returnOfSaleSummary.ReceiptQualityControlPassedQty = qualityControlPassedQty;

        if (receiptQualityControlFailedQty != null)
          returnOfSaleSummary.ReceiptQualityControlFailedQty = receiptQualityControlFailedQty;
        else if (returnOfSaleSummary.ReceiptQualityControlFailedQty != 0 && qualityControlPassedQty != 0)
        {
          returnOfSaleSummary.ReceiptQualityControlFailedQty = 0;
          returnOfSaleSummary.ReceiptQualityControlPassedQty = qualityControlPassedQty;
        }
        else if (returnOfSaleSummary.ReceiptQualityControlFailedQty == 0 && qualityControlFailedQty != 0)
          returnOfSaleSummary.ReceiptQualityControlFailedQty = qualityControlFailedQty;

        if (receiptQualityControlConsumedQty != null)
          returnOfSaleSummary.ReceiptQualityControlConsumedQty = receiptQualityControlConsumedQty;
        else if (returnOfSaleSummary.ReceiptQualityControlConsumedQty == 0 && qualityControlConsumedQty != 0)
          returnOfSaleSummary.ReceiptQualityControlConsumedQty = qualityControlConsumedQty;
      }
      else if (receipt == null || receipt.Status.HasFlag(ReceiptStatus.EternalReceipt) == false)
      {
        returnOfSaleSummary.ReceiptQualityControlPassedQty = returnOfSaleSummary.QualityControlPassedQty;
        returnOfSaleSummary.ReceiptQualityControlFailedQty = returnOfSaleSummary.QualityControlFailedQty;
        returnOfSaleSummary.ReceiptQualityControlConsumedQty = returnOfSaleSummary.QualityControlConsumedQty;
      }
      repository.Update(rowVersion: rowVersion, entity: returnOfSaleSummary);

      return returnOfSaleSummary;
    }

    #endregion
    #region Delete
    public void DeleteReturnOfSaleSummary(int id)
    {

      var returnOfSaleSummary = GetReturnOfSaleSummary(id: id);
      repository.Delete(returnOfSaleSummary);
    }
    #endregion
    #region Reset
    public ReturnOfSaleSummary ResetReturnOfSaleSummaryByreturnOfSaleId(int returnOfSaleId)
    {

      var returnOfSaleSummary = GetReturnOfSaleSummaryByReturnOfSaleId(returnOfSaleId: returnOfSaleId); ; return ResetReturnOfSaleSummary(returnOfSaleSummary: returnOfSaleSummary);

    }
    public ReturnOfSaleSummary ResetReturnOfSaleSummary(int id)
    {

      var returnOfSaleSummary = GetReturnOfSaleSummary(id: id); ; return ResetReturnOfSaleSummary(returnOfSaleSummary: returnOfSaleSummary);

    }
    public ReturnOfSaleSummary ResetReturnOfSaleSummary(ReturnOfSaleSummary returnOfSaleSummary)
    {

      var qualityControlItems = App.Internals.QualityControl.GetQualityControlItems(
                selector: e => new
                {
                  e.Id,
                  e.StuffId,
                  e.StuffSerialCode,
                  Qty = e.Qty * e.Unit.ConversionRatio,
                  DateTime = e.DateTime,
                  ReturnOfSaleConversionRatio = e.Unit.ConversionRatio,
                },
                isDelete: false,
                stuffId: returnOfSaleSummary.ReturnOfSale.StuffId,
                stuffSerialCode: returnOfSaleSummary.ReturnOfSale.StuffSerialCode)


             .Where(i => i.DateTime > returnOfSaleSummary.ReturnOfSale.ReturnStoreReceipt.DateTime);

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
                       stuffId: returnOfSaleSummary.ReturnOfSale.StuffId,
                       stuffSerialCode: returnOfSaleSummary.ReturnOfSale.StuffSerialCode
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
      ConditionalLastQty = ConditionalLastQtyQuery.Any() ? ConditionalLastQtyQuery.FirstOrDefault().Qty / returnOfSaleSummary.ReturnOfSale.Unit.ConversionRatio : 0;
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
      receiptAccepted = acceptedQuery.Any() ? acceptedQuery.Sum(i => i.RemainedQty) / returnOfSaleSummary.ReturnOfSale.Unit.ConversionRatio : 0;
      #endregion


      #region FailedQty
      var receiptFailed = 0d;
      var failedQuery = qualityControlConfirmationItemsGroup.Where(i => i.Status == QualityControlStatus.Rejected);
      receiptFailed = failedQuery.Any() ? failedQuery.Sum(i => i.RemainedQty) / returnOfSaleSummary.ReturnOfSale.Unit.ConversionRatio : 0;
      #endregion


      #region ConditionalQty
      var receiptConditional = 0d;
      var conditionalQuery = conditionalQualityControlItemsGroup.Where(i => i.Status == ConditionalQualityControlStatus.Accepted);
      receiptConditional = conditionalQuery.Any() ? conditionalQuery.Sum(i => i.Qty) / returnOfSaleSummary.ReturnOfSale.Unit.ConversionRatio : 0;
      #endregion                                 }).Sum(i => i.conditionalQty) / returnOfSaleSummary.ReturnOfSale.Unit.ConversionRatio;


      #region CounsumedQty
      var receiptConsumed = 0d;
      receiptConsumed = qualityControlConfirmationItemsGroup.Any() ? qualityControlConfirmationItemsGroup.Sum(i => i.ConsumeQty) / returnOfSaleSummary.ReturnOfSale.Unit.ConversionRatio : 0;
      #endregion

      var qualityControlPassedQty = 0d;
      var qualityControlFailedQty = 0d;
      var receiptQualityControlPassedQty = 0d;
      var receiptQualityControlFailedQty = 0d;
      var receiptQualityControlConsumedQty = 0d;

      var conditionalRemained = receiptFailed - receiptConditional;
      if (conditionalRemained < 0)
      {
        conditionalRemained = 0;
      }
      else if (conditionalRemained == 0)
      {
        if (receiptConditional > 0)
          conditionalRemained = ConditionalLastQty;
      }


      var qty = qualityControlItems.Max(i => (double?)i.Qty) ?? 0;

      qualityControlPassedQty = ((receiptAccepted > qty ? qty : receiptAccepted) + (conditionalRemained)) > qty
                                 ? qty
                                 : ((receiptAccepted > qty ? qty : receiptAccepted) + (conditionalRemained));

      qualityControlFailedQty = (receiptFailed - receiptConditional) < 0
                                ? 0
                                : receiptFailed - receiptConditional;

      if (maxqualityControlStatus == QualityControlStatus.Rejected && receiptConditional > 0)
      {
        qualityControlFailedQty = qty + (qualityControlFailedQty - qualityControlPassedQty);
        qualityControlPassedQty = qty - qualityControlFailedQty - receiptConsumed;
      }
      if (maxqualityControlStatus == QualityControlStatus.Accepted && receiptConditional > 0)
      {
        qualityControlPassedQty = qty + (qualityControlPassedQty - qualityControlFailedQty);
        qualityControlFailedQty = qty - qualityControlPassedQty - receiptConsumed;
      }


      if (returnOfSaleSummary.ReturnOfSale.ReturnStoreReceipt.Receipt == null || returnOfSaleSummary.ReturnOfSale.ReturnStoreReceipt.Receipt.Status != ReceiptStatus.EternalReceipt)
      {
        receiptQualityControlPassedQty = qualityControlPassedQty;
        receiptQualityControlFailedQty = qualityControlFailedQty;
        receiptQualityControlConsumedQty = receiptConsumed;
      }

      returnOfSaleSummary = EditReturnOfSaleSummary(
                 returnOfSaleSummary: returnOfSaleSummary,
                 rowVersion: returnOfSaleSummary.RowVersion,
                 qualityControlPassedQty: qualityControlPassedQty,
                 qualityControlFailedQty: qualityControlFailedQty,
                 qualityControlConsumedQty: receiptConsumed,
                 receiptQualityControlPassedQty: receiptQualityControlPassedQty,
                 receiptQualityControlFailedQty: receiptQualityControlFailedQty,
                 receiptQualityControlConsumedQty: receiptQualityControlConsumedQty
                 );

      return returnOfSaleSummary;

    }

    #endregion

  }
}
