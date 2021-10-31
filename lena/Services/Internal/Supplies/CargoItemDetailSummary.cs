using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
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
    #region GetByCargoItemDetailId 
    public CargoItemDetailSummary GetCargoItemDetailSummaryByCargoItemDetailId(int cargoItemDetailId) =>
    GetCargoItemDetailSummaryByCargoItemDetailId(selector: e => e, cargoItemDetailId: cargoItemDetailId);
    public TResult GetCargoItemDetailSummaryByCargoItemDetailId<TResult>(
        Expression<Func<CargoItemDetailSummary, TResult>> selector,
        int cargoItemDetailId)
    {

      var cargoItemDetailSummary = GetCargoItemDetailSummarys(
                    selector: selector,
                    cargoItemDetailId: cargoItemDetailId)

                .FirstOrDefault();

      if (cargoItemDetailSummary == null)
        throw new CargoItemDetailSummaryForCargoItemDetailNotFoundException(cargoItemDetailId: cargoItemDetailId);
      return cargoItemDetailSummary;
    }
    #endregion
    #region Get
    public CargoItemDetailSummary GetCargoItemDetailSummary(int id) => GetCargoItemDetailSummary(selector: e => e, id: id);
    public TResult GetCargoItemDetailSummary<TResult>(
        Expression<Func<CargoItemDetailSummary, TResult>> selector,
        int id)
    {

      var cargoItemDetailSummary = GetCargoItemDetailSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (cargoItemDetailSummary == null)
        throw new CargoItemDetailSummaryNotFoundException(id: id);
      return cargoItemDetailSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetCargoItemDetailSummarys<TResult>(
            Expression<Func<CargoItemDetailSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> receiptedQty = null,
            TValue<double> qualityControlPassedQty = null,
            TValue<double> qualityControlFailedQty = null,
            TValue<int> cargoItemDetailId = null)
    {

      var query = repository.GetQuery<CargoItemDetailSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (receiptedQty != null)
        query = query.Where(x => x.ReceiptedQty == receiptedQty);
      if (qualityControlPassedQty != null)
        query = query.Where(x => x.QualityControlPassedQty == qualityControlPassedQty);
      if (qualityControlFailedQty != null)
        query = query.Where(x => x.QualityControlFailedQty == qualityControlFailedQty);
      if (cargoItemDetailId != null)
        query = query.Where(x => x.CargoItemDetail.Id == cargoItemDetailId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public CargoItemDetailSummary AddCargoItemDetailSummary(
            double receiptedQty,
            double ladingItemDetailQty,
            double qualityControlPassedQty,
            double qualityControlFailedQty,
            int cargoItemDetailId)
    {

      var cargoItemDetailSummary = repository.Create<CargoItemDetailSummary>();
      cargoItemDetailSummary.ReceiptedQty = receiptedQty;
      cargoItemDetailSummary.LadingItemDetailQty = ladingItemDetailQty;
      cargoItemDetailSummary.QualityControlPassedQty = qualityControlPassedQty;
      cargoItemDetailSummary.QualityControlFailedQty = qualityControlFailedQty;
      cargoItemDetailSummary.CargoItemDetail = GetCargoItemDetail(id: cargoItemDetailId);
      repository.Add(cargoItemDetailSummary);
      return cargoItemDetailSummary;
    }
    #endregion
    #region Edit
    public CargoItemDetailSummary EditCargoItemDetailSummary(
        int id,
        byte[] rowVersion,
        TValue<double> receiptedQty = null,
        TValue<double> ladingItemDetailQty = null,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null)
    {

      var cargoItemDetailSummary = GetCargoItemDetailSummary(id: id);
      return EditCargoItemDetailSummary(
                    cargoItemDetailSummary: cargoItemDetailSummary,
                    rowVersion: rowVersion,
                    receiptedQty: receiptedQty,
                    ladingItemDetailQty: ladingItemDetailQty,
                    qualityControlPassedQty: qualityControlPassedQty,
                    qualityControlFailedQty: qualityControlFailedQty);
    }
    public CargoItemDetailSummary EditCargoItemDetailSummary(
        CargoItemDetailSummary cargoItemDetailSummary,
        byte[] rowVersion,
        TValue<double> receiptedQty = null,
        TValue<double> ladingItemDetailQty = null,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null)
    {

      if (receiptedQty != null)
        cargoItemDetailSummary.ReceiptedQty = receiptedQty;
      if (ladingItemDetailQty != null)
        cargoItemDetailSummary.LadingItemDetailQty = ladingItemDetailQty;
      if (qualityControlPassedQty != null)
        cargoItemDetailSummary.QualityControlPassedQty = qualityControlPassedQty;
      if (qualityControlFailedQty != null)
        cargoItemDetailSummary.QualityControlFailedQty = qualityControlFailedQty;
      repository.Update(rowVersion: rowVersion, entity: cargoItemDetailSummary);
      return cargoItemDetailSummary;
    }
    #endregion
    #region Delete
    public void DeleteCargoItemDetailSummary(int id)
    {

      var cargoItemDetailSummary = GetCargoItemDetailSummary(id: id);
      repository.Delete(cargoItemDetailSummary);
    }
    #endregion
    #region Reset
    public CargoItemDetailSummary ResetCargoItemDetailSummaryByCargoItemDetailId(int cargoItemDetailId)
    {

      var cargoItemDetailSummary = GetCargoItemDetailSummaryByCargoItemDetailId(cargoItemDetailId: cargoItemDetailId); ; return ResetCargoItemDetailSummary(cargoItemDetailSummary: cargoItemDetailSummary);

    }
    public CargoItemDetailSummary ResetCargoItemDetailSummary(int id)
    {

      var cargoItemDetailSummary = GetCargoItemDetailSummary(id: id); ; return ResetCargoItemDetailSummary(cargoItemDetailSummary: cargoItemDetailSummary);

    }
    public CargoItemDetailSummary ResetCargoItemDetailSummary(CargoItemDetailSummary cargoItemDetailSummary)
    {


      #region Get GetLadingItemDetails
      var ladingItemDetails = App.Internals.Supplies.GetLadingItemDetails(
              selector: e =>
                  new
                  {
                    ReceiptedQty = e.LadingItemDetailSummary.ReceiptedQty * e.CargoItemDetail.Unit.ConversionRatio / e.CargoItemDetail.Unit.ConversionRatio,
                    LadingItemDetailQty = e.Qty * e.CargoItemDetail.Unit.ConversionRatio / e.CargoItemDetail.Unit.ConversionRatio,
                    QualityControlPassedQty = e.LadingItemDetailSummary.QualityControlPassedQty * e.CargoItemDetail.Unit.ConversionRatio / e.CargoItemDetail.Unit.ConversionRatio,
                    QualityControlFailedQty = e.LadingItemDetailSummary.QualityControlFailedQty * e.CargoItemDetail.Unit.ConversionRatio / e.CargoItemDetail.Unit.ConversionRatio,
                  },
              cargoItemDetailId: cargoItemDetailSummary.CargoItemDetail.Id);


      var receiptedQty = 0d;
      var ladingItemDetailQty = 0d;
      var qualityControlPassedQty = 0d;
      var qualityControlFailedQty = 0d;
      if (ladingItemDetails.Any())
      {
        receiptedQty = ladingItemDetails.Sum(i => i.ReceiptedQty);
        ladingItemDetailQty = ladingItemDetails.Sum(i => i.LadingItemDetailQty);
        qualityControlPassedQty = ladingItemDetails.Sum(i => i.QualityControlPassedQty);
        qualityControlFailedQty = ladingItemDetails.Sum(i => i.QualityControlFailedQty);
      }
      #endregion
      #region EditCargoItemDetailSummary

      EditCargoItemDetailSummary(
              cargoItemDetailSummary: cargoItemDetailSummary,
              rowVersion: cargoItemDetailSummary.RowVersion,
              receiptedQty: receiptedQty,
              ladingItemDetailQty: ladingItemDetailQty,
              qualityControlPassedQty: qualityControlPassedQty,
              qualityControlFailedQty: qualityControlFailedQty);
      #endregion
      #region Get PurchaseOrderDetail
      var purchaseOrderDetail = GetCargoItemDetailSummary(
              selector: e => e.CargoItemDetail.PurchaseOrderDetail,
              id: cargoItemDetailSummary.Id);
      #endregion
      #region Reset PurchaseOrderDetailStatus
      if (purchaseOrderDetail != null)
        ResetPurchaseOrderDetailStatus(purchaseOrderDetail: purchaseOrderDetail);
      #endregion
      #region Get CargoItem
      var cargoItem = GetCargoItemDetailSummary(
              selector: e => e.CargoItemDetail.CargoItem,
              id: cargoItemDetailSummary.Id);
      #endregion
      #region Reset CargoItemStatus
      if (purchaseOrderDetail != null)
        ResetCargoItemStatus(cargoItem: cargoItem);
      #endregion
      return cargoItemDetailSummary;
    }

    #endregion

  }
}
