using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region GetByLadingItemDetailId 
    public LadingItemDetailSummary GetLadingItemDetailSummaryByLadingItemDetailId(int ladingItemDetailId) =>
    GetLadingItemDetailSummaryByLadingItemDetailId(selector: e => e, ladingItemDetailId: ladingItemDetailId);
    public TResult GetLadingItemDetailSummaryByLadingItemDetailId<TResult>(
        Expression<Func<LadingItemDetailSummary, TResult>> selector,
        int ladingItemDetailId)
    {

      var ladingItemDetailSummary = GetLadingItemDetailSummarys(
                    selector: selector,
                    ladingItemDetailId: ladingItemDetailId)


                .FirstOrDefault();
      if (ladingItemDetailSummary == null)
        throw new LadingItemDetailSummaryForLadingItemDetailNotFoundException(ladingItemDetailId: ladingItemDetailId);
      return ladingItemDetailSummary;
    }
    #endregion
    #region Get
    public LadingItemDetailSummary GetLadingItemDetailSummary(int id) => GetLadingItemDetailSummary(selector: e => e, id: id);
    public TResult GetLadingItemDetailSummary<TResult>(
        Expression<Func<LadingItemDetailSummary, TResult>> selector,
        int id)
    {

      var ladingItemDetailSummary = GetLadingItemDetailSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (ladingItemDetailSummary == null)
        throw new LadingItemDetailSummaryNotFoundException(id: id);
      return ladingItemDetailSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetLadingItemDetailSummarys<TResult>(
            Expression<Func<LadingItemDetailSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> receiptedQty = null,
            TValue<double> qualityControlPassedQty = null,
            TValue<double> qualityControlFailedQty = null,
            TValue<int> ladingItemDetailId = null)
    {

      var query = repository.GetQuery<LadingItemDetailSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (receiptedQty != null)
        query = query.Where(x => x.ReceiptedQty == receiptedQty);
      if (qualityControlPassedQty != null)
        query = query.Where(x => x.QualityControlPassedQty == qualityControlPassedQty);
      if (qualityControlFailedQty != null)
        query = query.Where(x => x.QualityControlFailedQty == qualityControlFailedQty);
      if (ladingItemDetailId != null)
        query = query.Where(x => x.LadingItemDetail.Id == ladingItemDetailId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public LadingItemDetailSummary AddLadingItemDetailSummary(
            double receiptedQty,
            double qualityControlPassedQty,
            double qualityControlFailedQty,
            int ladingItemDetailId)
    {

      var ladingItemDetailSummary = repository.Create<LadingItemDetailSummary>();
      ladingItemDetailSummary.ReceiptedQty = receiptedQty;
      ladingItemDetailSummary.QualityControlPassedQty = qualityControlPassedQty;
      ladingItemDetailSummary.QualityControlFailedQty = qualityControlFailedQty;
      ladingItemDetailSummary.LadingItemDetail = GetLadingItemDetail(id: ladingItemDetailId);
      repository.Add(ladingItemDetailSummary);
      return ladingItemDetailSummary;
    }
    #endregion
    #region Edit
    public LadingItemDetailSummary EditLadingItemDetailSummary(
        int id,
        byte[] rowVersion,
        TValue<double> receiptedQty = null,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null)
    {

      var ladingItemDetailSummary = GetLadingItemDetailSummary(id: id);
      return EditLadingItemDetailSummary(
                    ladingItemDetailSummary: ladingItemDetailSummary,
                    rowVersion: rowVersion,
                    receiptedQty: receiptedQty,
                    qualityControlPassedQty: qualityControlPassedQty,
                    qualityControlFailedQty: qualityControlFailedQty);
    }
    public LadingItemDetailSummary EditLadingItemDetailSummary(
        LadingItemDetailSummary ladingItemDetailSummary,
        byte[] rowVersion,
        TValue<double> receiptedQty = null,
        TValue<double> qualityControlPassedQty = null,
         TValue<double> qualityControlFailedQty = null)
    {

      if (receiptedQty != null)
        ladingItemDetailSummary.ReceiptedQty = receiptedQty;
      if (qualityControlPassedQty != null)
        ladingItemDetailSummary.QualityControlPassedQty = qualityControlPassedQty;
      if (qualityControlFailedQty != null)
        ladingItemDetailSummary.QualityControlFailedQty = qualityControlFailedQty;
      repository.Update(rowVersion: rowVersion, entity: ladingItemDetailSummary);
      return ladingItemDetailSummary;
    }
    #endregion
    #region Delete
    public void DeleteLadingItemDetailSummary(int id)
    {

      var ladingItemDetailSummary = GetLadingItemDetailSummary(id: id);
      repository.Delete(ladingItemDetailSummary);
    }
    #endregion
    #region Reset
    public LadingItemDetailSummary ResetLadingItemDetailSummaryByLadingItemDetailId(int ladingItemDetailId)
    {

      var ladingItemDetailSummary = GetLadingItemDetailSummaryByLadingItemDetailId(ladingItemDetailId: ladingItemDetailId); ; return ResetLadingItemDetailSummary(ladingItemDetailSummary: ladingItemDetailSummary);

    }
    public LadingItemDetailSummary ResetLadingItemDetailSummary(int id)
    {

      var ladingItemDetailSummary = GetLadingItemDetailSummary(id: id); ; return ResetLadingItemDetailSummary(ladingItemDetailSummary: ladingItemDetailSummary);

    }
    public LadingItemDetailSummary ResetLadingItemDetailSummary(LadingItemDetailSummary ladingItemDetailSummary)
    {


      #region Get GetLadingItemDetails
      var newShoppingDetails = App.Internals.WarehouseManagement.GetNewShoppingDetails(
              selector: e =>
                  new
                  {
                    ReceiptedQty = e.Qty * e.Unit.ConversionRatio / e.LadingItemDetail.CargoItemDetail.Unit.ConversionRatio,
                    QualityControlPassedQty = e.NewShoppingDetailSummary.QualityControlPassedQty * e.Unit.ConversionRatio / e.LadingItemDetail.CargoItemDetail.Unit.ConversionRatio,
                    QualityControlConsumedQty = e.NewShoppingDetailSummary.QualityControlConsumedQty * e.Unit.ConversionRatio / e.LadingItemDetail.CargoItemDetail.Unit.ConversionRatio,
                    QualityControlFailedQty = e.NewShoppingDetailSummary.QualityControlFailedQty * e.Unit.ConversionRatio / e.LadingItemDetail.CargoItemDetail.Unit.ConversionRatio,
                  },
              isDelete: false,
              ladingItemDetailId: ladingItemDetailSummary.LadingItemDetail.Id);

      var receiptedQty = 0d;
      var qualityControlPassedQty = 0d;
      var qualityControlFailedQty = 0d;
      if (newShoppingDetails.Any())
      {
        receiptedQty = newShoppingDetails.Sum(i => i.ReceiptedQty);
        qualityControlPassedQty = newShoppingDetails.Sum(i => i.QualityControlPassedQty);
        qualityControlFailedQty = newShoppingDetails.Sum(i => i.QualityControlFailedQty);
      }
      #endregion
      #region EditLadingItemDetailSummary
      EditLadingItemDetailSummary(
              ladingItemDetailSummary: ladingItemDetailSummary,
              rowVersion: ladingItemDetailSummary.RowVersion,
              receiptedQty: receiptedQty,
              qualityControlPassedQty: qualityControlPassedQty,
              qualityControlFailedQty: qualityControlFailedQty);
      #endregion
      #region Get CargoItemDetail
      var cargoItemDetail = GetCargoItemDetailSummary(
              selector: e => e.CargoItemDetail,
              id: ladingItemDetailSummary.LadingItemDetail.CargoItemDetail.CargoItemDetailSummary.Id);
      #endregion
      #region Reset CargoItemDetailsStatus
      if (cargoItemDetail != null)
        ResetCargoItemDetailStatus(cargoItemDetail: cargoItemDetail);
      #endregion
      #region Get LadingItem
      var ladingItem = GetLadingItemDetailSummary(
              selector: e => e.LadingItemDetail.LadingItem,
              id: ladingItemDetailSummary.Id);
      #endregion
      #region Reset LadingItemStatus
      if (cargoItemDetail != null)
        ResetLadingItemStatus(ladingItem: ladingItem);
      #endregion
      return ladingItemDetailSummary;
    }

    #endregion

  }
}
