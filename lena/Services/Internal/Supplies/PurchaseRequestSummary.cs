using System;
using System.Linq;
using System.Linq.Expressions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Get By PurchaseRequestId
    public PurchaseRequestSummary GetPurchaseRequestSummaryByPurchaseRequestId(int purchaseRequestId) =>
    GetPurchaseRequestSummaryByPurchaseRequestId(selector: e => e, purchaseRequestId: purchaseRequestId);
    public TResult GetPurchaseRequestSummaryByPurchaseRequestId<TResult>(
        Expression<Func<PurchaseRequestSummary, TResult>> selector,
        int purchaseRequestId)
    {

      var purchaseRequestSummary = GetPurchaseRequestSummarys(
                    selector: selector,
                    purchaseRequestId: purchaseRequestId)


                .FirstOrDefault();
      if (purchaseRequestSummary == null)
        throw new PurchaseRequestSummaryForPurchaseRequestNotFoundException(purchaseRequestId: purchaseRequestId);
      return purchaseRequestSummary;
    }
    #endregion
    #region Get
    public PurchaseRequestSummary GetPurchaseRequestSummary(int id) => GetPurchaseRequestSummary(selector: e => e, id: id);
    public TResult GetPurchaseRequestSummary<TResult>(
        Expression<Func<PurchaseRequestSummary, TResult>> selector,
        int id)
    {

      var purchaseRequestSummary = GetPurchaseRequestSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (purchaseRequestSummary == null)
        throw new PurchaseRequestSummaryNotFoundException(id);
      return purchaseRequestSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetPurchaseRequestSummarys<TResult>(
            Expression<Func<PurchaseRequestSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> orderedQty = null,
            TValue<double> cargoedQty = null,
            TValue<double> receiptedQty = null,
            TValue<double> qualityControlPassedQty = null,
            TValue<double> purchaseRequestId = null)
    {

      var query = repository.GetQuery<PurchaseRequestSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (orderedQty != null)
        query = query.Where(x => x.OrderedQty == orderedQty);
      if (cargoedQty != null)
        query = query.Where(x => x.CargoedQty == cargoedQty);
      if (receiptedQty != null)
        query = query.Where(x => x.ReceiptedQty == receiptedQty);
      if (qualityControlPassedQty != null)
        query = query.Where(x => x.QualityControlPassedQty == qualityControlPassedQty);
      if (purchaseRequestId != null)
        query = query.Where(x => x.PurchaseRequest.Id == purchaseRequestId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public PurchaseRequestSummary AddPurchaseRequestSummary(
            double orderedQty,
            double cargoedQty,
            double receiptedQty,
            double qualityControlPassedQty,
            double qualityControlFailedQty,
            int purchaseRequestId)
    {

      var purchaseRequestSummary = repository.Create<PurchaseRequestSummary>();
      purchaseRequestSummary.OrderedQty = orderedQty;
      purchaseRequestSummary.CargoedQty = cargoedQty;
      purchaseRequestSummary.ReceiptedQty = receiptedQty;
      purchaseRequestSummary.QualityControlPassedQty = qualityControlPassedQty;
      purchaseRequestSummary.QualityControlFailedQty = qualityControlFailedQty;
      purchaseRequestSummary.PurchaseRequest = GetPurchaseRequest(id: purchaseRequestId);
      repository.Add(purchaseRequestSummary);
      return purchaseRequestSummary;
    }
    #endregion
    #region Edit

    public PurchaseRequestSummary EditPurchaseRequestSummary(
        int id,
        byte[] rowVersion,
        TValue<double> orderedQty = null,
        TValue<double> cargoedQty = null,
        TValue<double> receiptedQty = null,
        TValue<double> qualityControlFailedQty = null,
        TValue<double> qualityControlPassedQty = null)
    {

      var purchaseRequestSummary = GetPurchaseRequestSummary(id: id);
      return EditPurchaseRequestSummary(
                    purchaseRequestSummary: purchaseRequestSummary,
                    rowVersion: rowVersion,
                    orderedQty: orderedQty,
                    cargoedQty: cargoedQty,
                    receiptedQty: receiptedQty,
                    qualityControlFailedQty: qualityControlFailedQty,
                    qualityControlPassedQty: qualityControlPassedQty);
    }

    public PurchaseRequestSummary EditPurchaseRequestSummary(
        PurchaseRequestSummary purchaseRequestSummary,
        byte[] rowVersion,
        TValue<double> orderedQty = null,
        TValue<double> cargoedQty = null,
        TValue<double> receiptedQty = null,
        TValue<double> qualityControlFailedQty = null,
        TValue<double> qualityControlPassedQty = null)
    {

      if (orderedQty != null)
        purchaseRequestSummary.OrderedQty = orderedQty;
      if (cargoedQty != null)
        purchaseRequestSummary.CargoedQty = cargoedQty;
      if (receiptedQty != null)
        purchaseRequestSummary.ReceiptedQty = receiptedQty;
      if (qualityControlPassedQty != null)
        purchaseRequestSummary.QualityControlPassedQty = qualityControlPassedQty;
      if (qualityControlFailedQty != null)
        purchaseRequestSummary.QualityControlFailedQty = qualityControlFailedQty;
      repository.Update(rowVersion: rowVersion, entity: purchaseRequestSummary);
      return purchaseRequestSummary;
    }

    #endregion
    #region Delete
    public void DeletePurchaseRequestSummary(int id)
    {

      var purchaseRequestSummary = GetPurchaseRequestSummary(id: id);
      repository.Delete(purchaseRequestSummary);
    }
    #endregion
    #region Reset
    public PurchaseRequestSummary ResetPurchaseRequestSummaryByPurchaseRequestId(int purchaseRequestId)
    {

      var purchaseRequestSummary = GetPurchaseRequestSummaryByPurchaseRequestId(purchaseRequestId: purchaseRequestId); ; return ResetPurchaseRequestSummary(purchaseRequestSummary: purchaseRequestSummary);

    }
    public PurchaseRequestSummary ResetPurchaseRequestSummary(int id)
    {

      var purchaseRequestSummary = GetPurchaseRequestSummary(id: id); ; return ResetPurchaseRequestSummary(purchaseRequestSummary: purchaseRequestSummary);

    }
    public PurchaseRequestSummary ResetPurchaseRequestSummary(PurchaseRequestSummary purchaseRequestSummary)
    {


      #region Get PurchaseRequests
      var purchaseRequestQtys = GetPurchaseOrderDetails(
              selector: e =>
                  new
                  {
                    OrderedQty = (Nullable<double>)Math.Round((e.Qty * e.Unit.ConversionRatio / e.PurchaseRequest.Unit.ConversionRatio), e.PurchaseRequest.Unit.DecimalDigitCount),
                    CargoedQty = (Nullable<double>)Math.Round((e.PurchaseOrderDetailSummary.CargoedQty * e.Unit.ConversionRatio / e.PurchaseRequest.Unit.ConversionRatio), e.PurchaseRequest.Unit.DecimalDigitCount),
                    ReceiptedQty = (Nullable<double>)Math.Round((e.PurchaseOrderDetailSummary.ReceiptedQty * e.Unit.ConversionRatio / e.PurchaseRequest.Unit.ConversionRatio), e.PurchaseRequest.Unit.DecimalDigitCount),
                    QualityControlPassedQty = (Nullable<double>)Math.Round((e.PurchaseOrderDetailSummary.QualityControlPassedQty * e.Unit.ConversionRatio / e.PurchaseRequest.Unit.ConversionRatio), e.PurchaseRequest.Unit.DecimalDigitCount),
                    QualityControlFailedQty = (Nullable<double>)Math.Round((e.PurchaseOrderDetailSummary.QualityControlFailedQty * e.Unit.ConversionRatio / e.PurchaseRequest.Unit.ConversionRatio), e.PurchaseRequest.Unit.DecimalDigitCount)
                  },
              isDelete: false,
              purchaseRequestId: purchaseRequestSummary.PurchaseRequest.Id);

      Nullable<double> orderedQty = 0d;
      Nullable<double> cargoedQty = 0d;
      Nullable<double> receiptedQty = 0d;
      Nullable<double> qualityControlPassedQty = 0d;
      Nullable<double> qualityControlFailedQty = 0d;
      if (purchaseRequestQtys.Any())
      {
        orderedQty = purchaseRequestQtys.Sum(i => i.OrderedQty);
        cargoedQty = purchaseRequestQtys.Sum(i => i.CargoedQty);
        receiptedQty = purchaseRequestQtys.Sum(i => i.ReceiptedQty);
        qualityControlPassedQty = purchaseRequestQtys.Sum(i => i.QualityControlPassedQty);
        qualityControlFailedQty = purchaseRequestQtys.Sum(i => i.QualityControlFailedQty);
      }
      #endregion

      #region EditPurchaseRequestSummary
      var purchaseRequest = GetPurchaseRequest(id: purchaseRequestSummary.PurchaseRequest.Id);
      EditPurchaseRequestSummary(
                    purchaseRequestSummary: purchaseRequestSummary,
                    rowVersion: purchaseRequestSummary.RowVersion,
                    orderedQty: Math.Round(orderedQty ?? 0, purchaseRequest.Unit.DecimalDigitCount),
                    cargoedQty: Math.Round(cargoedQty ?? 0, purchaseRequest.Unit.DecimalDigitCount),
                    receiptedQty: Math.Round(receiptedQty ?? 0, purchaseRequest.Unit.DecimalDigitCount),
                    qualityControlPassedQty: Math.Round(qualityControlPassedQty ?? 0, purchaseRequest.Unit.DecimalDigitCount),
                    qualityControlFailedQty: Math.Round(qualityControlFailedQty ?? 0, purchaseRequest.Unit.DecimalDigitCount));
      #endregion
      return purchaseRequestSummary;
    }

    #endregion

  }
}
