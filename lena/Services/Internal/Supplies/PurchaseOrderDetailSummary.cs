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
    #region Get
    public PurchaseOrderDetailSummary GetPurchaseOrderDetailSummaryByPurchaseOrderDetailId(int purchaseOrderDetailId) =>
    GetPurchaseOrderDetailSummaryByPurchaseOrderDetailId(selector: e => e, purchaseOrderDetailId: purchaseOrderDetailId);
    public TResult GetPurchaseOrderDetailSummaryByPurchaseOrderDetailId<TResult>(
        Expression<Func<PurchaseOrderDetailSummary, TResult>> selector,
        int purchaseOrderDetailId)
    {

      var purchaseOrderDetailSummary = GetPurchaseOrderDetailSummarys(
                    selector: selector,
                    purchaseOrderDetailId: purchaseOrderDetailId)


                .FirstOrDefault();
      if (purchaseOrderDetailSummary == null)
        throw new PurchaseOrderDetailSummaryForPurchaseOrderDetailNotFoundException(purchaseOrderDetailId: purchaseOrderDetailId);
      return purchaseOrderDetailSummary;
    }
    #endregion
    #region Get
    public PurchaseOrderDetailSummary GetPurchaseOrderDetailSummary(int id) => GetPurchaseOrderDetailSummary(selector: e => e, id: id);
    public TResult GetPurchaseOrderDetailSummary<TResult>(
        Expression<Func<PurchaseOrderDetailSummary, TResult>> selector,
        int id)
    {

      var purchaseOrderDetailSummary = GetPurchaseOrderDetailSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (purchaseOrderDetailSummary == null)
        throw new PurchaseOrderDetailSummaryNotFoundException(id: id);
      return purchaseOrderDetailSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetPurchaseOrderDetailSummarys<TResult>(
            Expression<Func<PurchaseOrderDetailSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> cargoedQty = null,
            TValue<double> receiptedQty = null,
            TValue<double> qualityControlPassedQty = null,
            TValue<int> purchaseOrderDetailId = null)
    {

      var query = repository.GetQuery<PurchaseOrderDetailSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (cargoedQty != null)
        query = query.Where(x => x.CargoedQty == cargoedQty);
      if (receiptedQty != null)
        query = query.Where(x => x.ReceiptedQty == receiptedQty);
      if (qualityControlPassedQty != null)
        query = query.Where(x => x.QualityControlPassedQty == qualityControlPassedQty);
      if (purchaseOrderDetailId != null)
        query = query.Where(x => x.PurchaseOrderDetail.Id == purchaseOrderDetailId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public PurchaseOrderDetailSummary AddPurchaseOrderDetailSummary(
            double cargoedQty,
            double receiptedQty,
            double qualityControlPassedQty,
            double qualityControlFailedQty,
            int purchaseOrderDetailId)
    {

      var purchaseOrderDetailSummary = repository.Create<PurchaseOrderDetailSummary>();
      purchaseOrderDetailSummary.CargoedQty = cargoedQty;
      purchaseOrderDetailSummary.ReceiptedQty = receiptedQty;
      purchaseOrderDetailSummary.QualityControlPassedQty = qualityControlPassedQty;
      purchaseOrderDetailSummary.QualityControlFailedQty = qualityControlFailedQty;
      purchaseOrderDetailSummary.PurchaseOrderDetail = GetPurchaseOrderDetail(id: purchaseOrderDetailId);
      repository.Add(purchaseOrderDetailSummary);
      return purchaseOrderDetailSummary;
    }
    #endregion
    #region Edit

    public PurchaseOrderDetailSummary EditPurchaseOrderDetailSummary(
        int id,
        byte[] rowVersion,
        TValue<double> cargoedQty = null,
        TValue<double> receiptedQty = null,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null)
    {

      var purchaseOrderDetailSummary = GetPurchaseOrderDetailSummary(id: id);
      return EditPurchaseOrderDetailSummary(
                    purchaseOrderDetailSummary: purchaseOrderDetailSummary,
                    rowVersion: rowVersion,
                    cargoedQty: cargoedQty,
                    receiptedQty: receiptedQty,
                    qualityControlPassedQty: qualityControlPassedQty,
                    qualityControlFailedQty: qualityControlFailedQty);
    }

    public PurchaseOrderDetailSummary EditPurchaseOrderDetailSummary(
        PurchaseOrderDetailSummary purchaseOrderDetailSummary,
        byte[] rowVersion,

        TValue<double> cargoedQty = null,
        TValue<double> receiptedQty = null,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null)
    {


      if (cargoedQty != null)
        purchaseOrderDetailSummary.CargoedQty = cargoedQty;
      if (receiptedQty != null)
        purchaseOrderDetailSummary.ReceiptedQty = receiptedQty;
      if (qualityControlPassedQty != null)
        purchaseOrderDetailSummary.QualityControlPassedQty = qualityControlPassedQty;
      if (qualityControlFailedQty != null)
        purchaseOrderDetailSummary.QualityControlFailedQty = qualityControlFailedQty;
      repository.Update(rowVersion: rowVersion, entity: purchaseOrderDetailSummary);
      return purchaseOrderDetailSummary;
    }

    #endregion
    #region Delete
    public void DeletePurchaseOrderDetailSummary(int id)
    {

      var purchaseOrderDetailSummary = GetPurchaseOrderDetailSummary(id: id);
      repository.Delete(purchaseOrderDetailSummary);
    }
    #endregion
    #region Reset
    public PurchaseOrderDetailSummary ResetPurchaseOrderDetailSummaryByPurchaseOrderDetailId(int purchaseOrderDetailId)
    {

      var purchaseOrderDetailSummary = GetPurchaseOrderDetailSummaryByPurchaseOrderDetailId(purchaseOrderDetailId: purchaseOrderDetailId); ; return ResetPurchaseOrderDetailSummary(purchaseOrderDetailSummary: purchaseOrderDetailSummary);

    }
    public PurchaseOrderDetailSummary ResetPurchaseOrderDetailSummary(int id)
    {

      var purchaseOrderDetailSummary = GetPurchaseOrderDetailSummary(id: id); ; return ResetPurchaseOrderDetailSummary(purchaseOrderDetailSummary: purchaseOrderDetailSummary);

    }
    public PurchaseOrderDetailSummary ResetPurchaseOrderDetailSummary(PurchaseOrderDetailSummary purchaseOrderDetailSummary)
    {


      #region Get CargoItemDetails
      var cargoItemDetails = GetCargoItemDetails(
              selector: e =>
                  new
                  {
                    CargoedQty = e.Qty * e.Unit.ConversionRatio / e.PurchaseOrderDetail.Unit.ConversionRatio,
                    ReceiptedQty = e.CargoItemDetailSummary.ReceiptedQty * e.Unit.ConversionRatio / e.PurchaseOrderDetail.Unit.ConversionRatio,
                    QualityControlPassedQty = e.CargoItemDetailSummary.QualityControlPassedQty * e.Unit.ConversionRatio / e.PurchaseOrderDetail.Unit.ConversionRatio,
                    QualityControlFailedQty = e.CargoItemDetailSummary.QualityControlFailedQty * e.Unit.ConversionRatio / e.PurchaseOrderDetail.Unit.ConversionRatio
                  },
              isDelete: false,
              purchaseOrderDetailId: purchaseOrderDetailSummary.PurchaseOrderDetail.Id);


      var cargoedQty = 0d;
      var receiptedQty = 0d;
      var qualityControlPassedQty = 0d;
      var qualityControlFailedQty = 0d;
      if (cargoItemDetails.Any())
      {
        cargoedQty = cargoItemDetails.Sum(i => i.CargoedQty);
        receiptedQty = cargoItemDetails.Sum(i => i.ReceiptedQty);
        qualityControlPassedQty = cargoItemDetails.Sum(i => i.QualityControlPassedQty);
        qualityControlFailedQty = cargoItemDetails.Sum(i => i.QualityControlFailedQty);
      }
      #endregion
      #region EditPurchaseOrderDetailSummary

      EditPurchaseOrderDetailSummary(
              purchaseOrderDetailSummary: purchaseOrderDetailSummary,
              rowVersion: purchaseOrderDetailSummary.RowVersion,
              cargoedQty: cargoedQty,
              receiptedQty: receiptedQty,
              qualityControlPassedQty: qualityControlPassedQty,
              qualityControlFailedQty: qualityControlFailedQty);
      #endregion
      #region Get PurchaseRequest
      var purchaseRequest = GetPurchaseOrderDetailSummary(
              selector: e => e.PurchaseOrderDetail.PurchaseRequest,
              id: purchaseOrderDetailSummary.Id);
      #endregion
      #region Reset PurchaseRequestStatus
      if (purchaseRequest != null)
      {
        ResetPurchaseRequestStatus(purchaseRequest: purchaseRequest);
      }
      #endregion
      #region Get PurchaseOrder
      var purchaseOrder = GetPurchaseOrderDetailSummary(
              selector: e => e.PurchaseOrderDetail.PurchaseOrder,
              id: purchaseOrderDetailSummary.Id);
      #endregion
      #region Reset PurchaseOrderStatus
      ResetPurchaseOrderStatus(purchaseOrder: purchaseOrder);

      #endregion

      return purchaseOrderDetailSummary;
    }

    #endregion
  }
}
