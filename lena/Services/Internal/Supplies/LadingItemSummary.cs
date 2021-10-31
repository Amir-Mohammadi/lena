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

    #region Get ByCargoItemId
    public LadingItemSummary GetLadingItemSummaryByLadingItemId(int ladingItemId) => GetLadingItemSummaryByLadingItemId(selector: e => e, ladingItemId: ladingItemId);
    public TResult GetLadingItemSummaryByLadingItemId<TResult>(
        Expression<Func<LadingItemSummary, TResult>> selector,
        int ladingItemId)
    {

      var ladingItemSummary = GetLadingItemSummarys(
                    selector: selector,
                    ladingItemId: ladingItemId)


                .FirstOrDefault();
      if (ladingItemSummary == null)
        throw new LadingItemSummaryForLadingItemNotFoundException(ladingItemId: ladingItemId);
      return ladingItemSummary;
    }
    #endregion
    #region Get
    public LadingItemSummary GetLadingItemSummary(int id) => GetLadingItemSummary(selector: e => e, id: id);
    public TResult GetLadingItemSummary<TResult>(
        Expression<Func<LadingItemSummary, TResult>> selector,
        int id)
    {

      var ladingItemSummary = GetLadingItemSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (ladingItemSummary == null)
        throw new LadingItemSummaryNotFoundException(id: id);
      return ladingItemSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetLadingItemSummarys<TResult>(
            Expression<Func<LadingItemSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> receiptedQty = null,
            TValue<double> qualityControlPassedQty = null,
            TValue<double> qualityControlFailedQty = null,
            TValue<int> ladingItemId = null)
    {

      var query = repository.GetQuery<LadingItemSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (receiptedQty != null)
        query = query.Where(x => x.ReceiptedQty == receiptedQty);
      if (qualityControlPassedQty != null)
        query = query.Where(x => x.QualityControlPassedQty == qualityControlPassedQty);
      if (qualityControlFailedQty != null)
        query = query.Where(x => x.QualityControlFailedQty == qualityControlFailedQty);
      if (ladingItemId != null)
        query = query.Where(x => x.LadingItem.Id == ladingItemId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public LadingItemSummary AddLadingItemSummary(
            double receiptedQty,
            double qualityControlPassedQty,
            double qualityControlFailedQty,
            int ladingItemId)
    {

      var ladingItemSummary = repository.Create<LadingItemSummary>();
      ladingItemSummary.ReceiptedQty = receiptedQty;
      ladingItemSummary.QualityControlPassedQty = qualityControlPassedQty;
      ladingItemSummary.QualityControlFailedQty = qualityControlFailedQty;
      ladingItemSummary.LadingItem = GetLadingItem(id: ladingItemId);
      repository.Add(ladingItemSummary);
      return ladingItemSummary;
    }
    #endregion
    #region Edit
    public LadingItemSummary EditLadingItemSummary(
        int id,
        byte[] rowVersion,
        TValue<double> receiptedQty = null,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null)
    {

      var ladingItemSummary = GetLadingItemSummary(id: id);
      return EditLadingItemSummary(
                    ladingItemSummary: ladingItemSummary,
                    rowVersion: rowVersion,
                    receiptedQty: receiptedQty,
                    qualityControlPassedQty: qualityControlPassedQty,
                    qualityControlFailedQty: qualityControlFailedQty);
    }
    public LadingItemSummary EditLadingItemSummary(
        LadingItemSummary ladingItemSummary,
        byte[] rowVersion,
        TValue<double> receiptedQty = null,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null)
    {

      if (receiptedQty != null)
        ladingItemSummary.ReceiptedQty = receiptedQty;
      if (qualityControlPassedQty != null)
        ladingItemSummary.QualityControlPassedQty = qualityControlPassedQty;
      if (qualityControlFailedQty != null)
        ladingItemSummary.QualityControlFailedQty = qualityControlFailedQty;
      repository.Update(rowVersion: rowVersion, entity: ladingItemSummary);
      return ladingItemSummary;
    }
    #endregion
    #region Delete
    public void DeleteLadingItemSummary(int id)
    {

      var ladingItemSummary = GetLadingItemSummary(id: id);
      repository.Delete(ladingItemSummary);
    }
    #endregion



    #region Reset
    public LadingItemSummary ResetLadingItemSummaryByLadingItemId(int ladingItemId)
    {

      var ladingItemSummary = GetLadingItemSummaryByLadingItemId(ladingItemId: ladingItemId); ; return ResetLadingItemSummary(ladingItemSummary: ladingItemSummary);

    }
    public LadingItemSummary ResetLadingItemSummary(int id)
    {

      var ladingItemSummary = GetLadingItemSummary(id: id); ; return ResetLadingItemSummary(ladingItemSummary: ladingItemSummary);

    }
    public LadingItemSummary ResetLadingItemSummary(LadingItemSummary ladingItemSummary)
    {

      #region Get CargoItems
      var newShoppings = App.Internals.WarehouseManagement.GetNewShoppings(
              selector: e =>
                  new
                  {
                    ReceiptedQty = (double?)e.Amount * e.Unit.ConversionRatio / e.LadingItem.CargoItem.Unit.ConversionRatio,
                    QualityControlPassedQty = (double?)e.StoreReceiptSummary.QualityControlPassedQty * e.Unit.ConversionRatio / e.LadingItem.CargoItem.Unit.ConversionRatio,
                    QualityControlFailedQty = (double?)e.StoreReceiptSummary.QualityControlFailedQty * e.Unit.ConversionRatio / e.LadingItem.CargoItem.Unit.ConversionRatio
                  },
              ladingItemId: ladingItemSummary.LadingItem.Id,
              isDelete: false);
      var receiptedQty = 0d;
      var qualityControlPassedQty = 0d;
      var qualityControlFailedQty = 0d;
      var x = 0;
      if (newShoppings.Any())
      {
        x = newShoppings.Count();
        receiptedQty = newShoppings.Sum(i => i.ReceiptedQty ?? 0);
        qualityControlPassedQty = newShoppings.Sum(i => i.QualityControlPassedQty ?? 0);
        qualityControlFailedQty = newShoppings.Sum(i => i.QualityControlFailedQty ?? 0);
      }
      #endregion
      #region EditLadingItemSummary
      EditLadingItemSummary(
              ladingItemSummary: ladingItemSummary,
              rowVersion: ladingItemSummary.RowVersion,
              receiptedQty: receiptedQty,
              qualityControlPassedQty: qualityControlPassedQty,
              qualityControlFailedQty: qualityControlFailedQty);
      #endregion
      #region GetCargoItem  
      //آیا نیازی به ریست کردن هست؟؟
      var cargoItem = GetLadingItemSummary(
          selector: e => e.LadingItem.CargoItem,
          id: ladingItemSummary.Id);
      #endregion
      #region Reset CargoItemStatus
      ResetCargoItemStatus(cargoItem: cargoItem);
      #endregion

      return ladingItemSummary;
    }

    #endregion

  }
}
