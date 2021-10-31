using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Get By QualityControlId
    public QualityControlSummary GetQualityControlSummaryByQualityControlId(int qualityControlId) =>
     GetQualityControlSummaryByQualityControlId(selector: e => e, qualityControlId: qualityControlId);
    public TResult GetQualityControlSummaryByQualityControlId<TResult>(
        Expression<Func<QualityControlSummary, TResult>> selector,
        int qualityControlId)
    {

      var qualityControlSummary = GetQualityControlSummarys(
                    selector: selector,
                    qualityControlId: qualityControlId)


                .FirstOrDefault();
      if (qualityControlSummary == null)
        throw new QualityControlSummaryForQualityControlNotFoundException(id: qualityControlId);
      return qualityControlSummary;
    }
    #endregion
    #region Get
    public QualityControlSummary GetQualityControlSummary(int id) => GetQualityControlSummary(selector: e => e, id: id);
    public TResult GetQualityControlSummary<TResult>(
        Expression<Func<QualityControlSummary, TResult>> selector,
        int id)
    {

      var qualityControlSummary = GetQualityControlSummarys(
                   selector: selector,
                   id: id)


               .FirstOrDefault();
      if (qualityControlSummary == null)
        throw new QualityControlSummaryNotFoundException(id: id);
      return qualityControlSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetQualityControlSummarys<TResult>(
            Expression<Func<QualityControlSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> acceptedQty = null,
            TValue<double> failedQty = null,
            TValue<double> conditionalRequestQty = null,
            TValue<double> conditionalQty = null,
            TValue<double> conditionalRejectedQty = null,
            TValue<double> returnedQty = null,
            TValue<double> consumedQty = null,
            TValue<int> qualityControlId = null)
    {

      var query = repository.GetQuery<QualityControlSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (acceptedQty != null)
        query = query.Where(x => x.AcceptedQty == acceptedQty);
      if (failedQty != null)
        query = query.Where(x => x.FailedQty == failedQty);
      if (conditionalRequestQty != null)
        query = query.Where(x => x.ConditionalRequestQty == conditionalRequestQty);
      if (conditionalQty != null)
        query = query.Where(x => x.ConditionalQty == conditionalQty);
      if (conditionalRejectedQty != null)
        query = query.Where(x => x.ConditionalRejectedQty == conditionalRejectedQty);
      if (returnedQty != null)
        query = query.Where(x => x.ReturnedQty == returnedQty);
      if (consumedQty != null)
        query = query.Where(x => x.ConsumedQty == consumedQty);
      if (qualityControlId != null)
        query = query.Where(x => x.QualityControl.Id == qualityControlId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public QualityControlSummary AddQualityControlSummary(
            double acceptedQty,
            double failedQty,
            double conditionalRequestQty,
            double conditionalQty,
            double conditionalRejectedQty,
            double returnedQty,
            double consumedQty,
            int qualityControlId)
    {

      var qualityControlSummary = repository.Create<QualityControlSummary>();
      qualityControlSummary.AcceptedQty = acceptedQty;
      qualityControlSummary.FailedQty = failedQty;
      qualityControlSummary.ConditionalRequestQty = conditionalRequestQty;
      qualityControlSummary.ConditionalQty = conditionalQty;
      qualityControlSummary.ConditionalRejectedQty = conditionalRejectedQty;
      qualityControlSummary.ReturnedQty = returnedQty;
      qualityControlSummary.ConsumedQty = consumedQty;
      qualityControlSummary.QualityControl = GetQualityControl(id: qualityControlId);
      repository.Add(qualityControlSummary);
      return qualityControlSummary;
    }
    #endregion
    #region Edit
    public QualityControlSummary EditQualityControlSummary(
        int id,
        byte[] rowVersion,
        TValue<double> acceptedQty = null,
        TValue<double> failedQty = null,
        TValue<double> conditionalRequestQty = null,
        TValue<double> conditionalQty = null,
        TValue<double> conditionalRejectedQty = null,
        TValue<double> returnedQty = null,
        TValue<double> consumedQty = null)
    {

      var qualityControlSummary = GetQualityControlSummary(id: id);
      return EditQualityControlSummary(
                    qualityControlSummary: qualityControlSummary,
                    rowVersion: rowVersion,
                    acceptedQty: acceptedQty,
                    failedQty: failedQty,
                    conditionalRequestQty: conditionalRequestQty,
                    conditionalQty: conditionalQty,
                    conditionalRejectedQty: conditionalRejectedQty,
                    returnedQty: returnedQty,
                    consumedQty: consumedQty);
    }

    public QualityControlSummary EditQualityControlSummary(
                QualityControlSummary qualityControlSummary,
                byte[] rowVersion,
                TValue<double> acceptedQty = null,
                TValue<double> failedQty = null,
                TValue<double> conditionalRequestQty = null,
                TValue<double> conditionalQty = null,
                TValue<double> conditionalRejectedQty = null,
                TValue<double> returnedQty = null,
                TValue<double> consumedQty = null)
    {

      if (acceptedQty != null)
        qualityControlSummary.AcceptedQty = acceptedQty;
      if (failedQty != null)
        qualityControlSummary.FailedQty = failedQty;
      if (conditionalRequestQty != null)
        qualityControlSummary.ConditionalRequestQty = conditionalRequestQty;
      if (conditionalQty != null)
        qualityControlSummary.ConditionalQty = conditionalQty;
      if (conditionalRejectedQty != null)
        qualityControlSummary.ConditionalRejectedQty = conditionalRejectedQty;
      if (returnedQty != null)
        qualityControlSummary.ReturnedQty = returnedQty;
      if (consumedQty != null)
        qualityControlSummary.ConsumedQty = consumedQty;
      repository.Update(rowVersion: rowVersion, entity: qualityControlSummary);
      return qualityControlSummary;
    }

    #endregion
    #region Delete
    public void DeleteQualityControlSummary(int id)
    {

      var qualityControlSummary = GetQualityControlSummary(id: id);
      repository.Delete(qualityControlSummary);
    }
    #endregion
    #region Reset
    public QualityControlSummary ResetQualityControlSummaryByQualityControlId(int qualityControlId)
    {

      var qualityControlSummary = GetQualityControlSummaryByQualityControlId(qualityControlId: qualityControlId); ; return ResetQualityControlSummary(qualityControlSummary: qualityControlSummary);

    }
    public QualityControlSummary ResetQualityControlSummary(int id)
    {

      var qualityControlSummary = GetQualityControlSummary(id: id); ; return ResetQualityControlSummary(qualityControlSummary: qualityControlSummary);

    }
    public QualityControlSummary ResetQualityControlSummary(QualityControlSummary qualityControlSummary)
    {


      var qualityControl = qualityControlSummary.QualityControl;
      var unitId = qualityControl.UnitId;
      var unitConversionRatio = qualityControl.Unit.ConversionRatio;
      #region Get QualityControlQtys
      var acceptedQty = 0d;
      var failedQty = 0d;
      var conditionalRequestQty = 0d;
      var conditionalQty = 0d;
      var conditionalRejectedQty = 0d;
      var conditionalNoReviewsQty = 0d;
      var returnedQty = 0d;
      var consumedQty = 0d;
      #region Get QualityControlItems
      var qualityControlItems = GetQualityControlItems(
              selector: e => new
              {
                e.StuffId,
                e.StuffSerialCode,
                Qty = e.Qty * e.Unit.ConversionRatio
              },
              isDelete: false,
              qualityControlId: qualityControl.Id);
      #endregion
      #region Get QualityControlConfirmationItems
      var qualityControlConfirmationItems = GetQualityControlConfirmationItems(
              selector: e => new
              {
                ConsumeQty = e.ConsumeQty * e.Unit.ConversionRatio,
                RemainedQty = e.RemainedQty * e.Unit.ConversionRatio,
                Status = e.QualityControlConfirmation.Status
              },
              isDelete: false,
              qualityControlId: qualityControl.Id);
      #endregion
      #region Get ConditionalRequestQualityControlitems
      var conditionalQualityControlItems = GetConditionalQualityControlItems(
              selector: e => new
              {
                Qty = e.Qty * e.Unit.ConversionRatio,
              },
              isDelete: false,
              qualityControlId: qualityControl.Id);
      #endregion
      #region Get ResponseConditionalQualityControls
      var responseConditionalQualityControls = GetResponseConditionalQualityControls(
              selector: e => new
              {
                ConditionalQualityControlItems = e.ConditionalQualityControl.ConditionalQualityControlItems,
              },
              isDelete: false,
              status: ConditionalQualityControlStatus.Accepted,
              qualityControlId: qualityControl.Id);
      #endregion
      #region Get RejectedConditionalQualityControls
      var rejectedConditionalQualityControls = GetResponseConditionalQualityControls(
              selector: e => new
              {
                ConditionalQualityControlItems = e.ConditionalQualityControl.ConditionalQualityControlItems,
              },
              isDelete: false,
              status: ConditionalQualityControlStatus.Rejected,
              qualityControlId: qualityControl.Id);
      #endregion
      #region Get NoReviewsConditionalQualityControls
      var noReviewsConditionalQualityControls = GetResponseConditionalQualityControls(
              selector: e => new
              {
                ConditionalQualityControlItems = e.ConditionalQualityControl.ConditionalQualityControlItems,
              },
              isDelete: false,
              status: ConditionalQualityControlStatus.NoReviews,
              qualityControlId: qualityControl.Id);
      #endregion

      #region Get GiveBackExitReceiptRequest

      var giveBackExitReceiptRequests = App.Internals.WarehouseManagement.GetGiveBackExitReceiptRequests(selector: e => e,
          stuffId: qualityControl.StuffId,
          isDelete: false,
          qualitControlId: qualityControl.Id);

      #endregion

      #region Qty
      var qty = qualityControlItems.Sum(i => (double?)i.Qty) ?? 0;
      #endregion
      #region AcceptedQuery
      var acceptedQuery = qualityControlConfirmationItems.Where(i => i.Status == QualityControlStatus.Accepted);
      acceptedQty = acceptedQuery.Any() ? acceptedQuery.Sum(i => i.RemainedQty) / unitConversionRatio : 0;
      #endregion
      #region FailedQty
      var failedQuery = qualityControlConfirmationItems.Where(i => i.Status == QualityControlStatus.Rejected);
      failedQty = failedQuery.Any() ? failedQuery.Sum(i => i.RemainedQty) / unitConversionRatio : 0;
      #endregion
      #region ConditionalRequestQty
      conditionalRequestQty = conditionalQualityControlItems.Any()
          ? conditionalQualityControlItems.Sum(i => i.Qty) / unitConversionRatio
          : 0;
      #endregion
      #region ConditionalQty
      var conditionalRequestQuery = from item in responseConditionalQualityControls
                                    from conditionalItem in item.ConditionalQualityControlItems
                                    select new
                                    {
                                      Qty = conditionalItem.Qty * conditionalItem.Unit.ConversionRatio,

                                    };
      conditionalQty = conditionalRequestQuery.Any()
                ? conditionalRequestQuery.Sum(i => i.Qty) / unitConversionRatio
                : 0;
      #endregion
      #region ConditionalRejectedQty
      var conditionalRejectedQuery = from item in rejectedConditionalQualityControls
                                     from conditionalItem in item.ConditionalQualityControlItems
                                     select new
                                     {
                                       Qty = conditionalItem.Qty * conditionalItem.Unit.ConversionRatio,

                                     };
      conditionalRejectedQty = conditionalRejectedQuery.Any()
                ? conditionalRejectedQuery.Sum(i => i.Qty) / unitConversionRatio
                : 0;
      #endregion
      #region ConditionalNoReviewsQty
      var conditionalNoReviewsQuery = from item in noReviewsConditionalQualityControls
                                      from conditionalItem in item.ConditionalQualityControlItems
                                      select new
                                      {
                                        Qty = conditionalItem.Qty * conditionalItem.Unit.ConversionRatio,

                                      };
      conditionalNoReviewsQty = conditionalNoReviewsQuery.Any()
                ? conditionalNoReviewsQuery.Sum(i => i.Qty) / unitConversionRatio
                : 0;
      #endregion
      #region ConsumedQty
      consumedQty = qualityControlConfirmationItems.Any() ? qualityControlConfirmationItems.Sum(i => i.ConsumeQty) / unitConversionRatio : 0;
      #endregion

      #region returnedQty
      returnedQty = giveBackExitReceiptRequests.Any() ? giveBackExitReceiptRequests.Sum(i => i.Qty) / unitConversionRatio : 0;
      #endregion

      #endregion
      #region EditQualityControlSummary

      EditQualityControlSummary(
              qualityControlSummary: qualityControlSummary,
              rowVersion: qualityControlSummary.RowVersion,
              acceptedQty: acceptedQty,
              failedQty: failedQty,
              conditionalRequestQty: conditionalRequestQty,
              conditionalQty: conditionalQty,
              conditionalRejectedQty: conditionalRejectedQty + conditionalNoReviewsQty,
              returnedQty: returnedQty,
              consumedQty: consumedQty);
      #endregion

      var storeReceiptId = (qualityControl as ReceiptQualityControl)?.StoreReceiptId;
      if (storeReceiptId != null)
      {
        #region ResetNewShoppingSummary
        App.Internals.WarehouseManagement.ResetStoreReceiptStatus(storeReceiptId: storeReceiptId.Value);
        #endregion
      }
      else
      {
        foreach (var qualityControlItem in qualityControlItems)
        {
          var storeReceiptIds = GetQualityControlItems(selector: e => (int?)(e.QualityControl as ReceiptQualityControl).StoreReceiptId,
                        stuffId: qualityControlItem.StuffId,
                        stuffSerialCode: qualityControlItem.StuffSerialCode,
                        qualityControlType: QualityControlType.ReceiptQualityControl)


                    .Distinct();
          foreach (var srId in storeReceiptIds)
          {
            #region ResetNewShoppingSummary
            App.Internals.WarehouseManagement.ResetStoreReceiptStatus(storeReceiptId: srId.Value);
            #endregion
          }
        }
      }

      return qualityControlSummary;
    }

    #endregion
  }
}
