using System;
using System.Linq;
using System.Linq.Expressions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Core;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Get ByCargoItemId
    public CargoItemSummary GetCargoItemSummaryByCargoItemId(int cargoItemId) => GetCargoItemSummaryByCargoItemId(selector: e => e, cargoItemId: cargoItemId);
    public TResult GetCargoItemSummaryByCargoItemId<TResult>(
        Expression<Func<CargoItemSummary, TResult>> selector,
        int cargoItemId)
    {

      var cargoItemSummary = GetCargoItemSummarys(
                    selector: selector,
                    cargoItemId: cargoItemId)


                .FirstOrDefault();
      if (cargoItemSummary == null)
        throw new CargoItemSummaryForCargoItemNotFoundException(cargoItemId: cargoItemId);
      return cargoItemSummary;
    }
    #endregion
    #region Get
    public CargoItemSummary GetCargoItemSummary(int id) => GetCargoItemSummary(selector: e => e, id: id);
    public TResult GetCargoItemSummary<TResult>(
        Expression<Func<CargoItemSummary, TResult>> selector,
        int id)
    {

      var cargoItemSummary = GetCargoItemSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (cargoItemSummary == null)
        throw new CargoItemSummaryNotFoundException(id: id);
      return cargoItemSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetCargoItemSummarys<TResult>(
            Expression<Func<CargoItemSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> receiptedQty = null,
            TValue<double> qualityControlPassedQty = null,
            TValue<double> qualityControlFailedQty = null,
            TValue<int> cargoItemId = null)
    {

      var query = repository.GetQuery<CargoItemSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (receiptedQty != null)
        query = query.Where(x => x.ReceiptedQty == receiptedQty);
      if (qualityControlPassedQty != null)
        query = query.Where(x => x.QualityControlPassedQty == qualityControlPassedQty);
      if (qualityControlFailedQty != null)
        query = query.Where(x => x.QualityControlFailedQty == qualityControlFailedQty);
      if (cargoItemId != null)
        query = query.Where(x => x.CargoItem.Id == cargoItemId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public CargoItemSummary AddCargoItemSummary(
            double receiptedQty,
            double ladingItemQty,
            double qualityControlPassedQty,
            double qualityControlFailedQty,
            int cargoItemId)
    {

      var cargoItemSummary = repository.Create<CargoItemSummary>();
      cargoItemSummary.ReceiptedQty = receiptedQty;
      cargoItemSummary.LadingItemQty = ladingItemQty;
      cargoItemSummary.QualityControlPassedQty = qualityControlPassedQty;
      cargoItemSummary.QualityControlFailedQty = qualityControlFailedQty;
      cargoItemSummary.CargoItem = GetCargoItem(id: cargoItemId);
      repository.Add(cargoItemSummary);
      return cargoItemSummary;
    }
    #endregion
    #region Edit
    public CargoItemSummary EditCargoItemSummary(
        int id,
        byte[] rowVersion,
        TValue<double> receiptedQty = null,
        TValue<double> ladingItemQty = null,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null)
    {

      var cargoItemSummary = GetCargoItemSummary(id: id);
      return EditCargoItemSummary(
                    cargoItemSummary: cargoItemSummary,
                    rowVersion: rowVersion,
                    receiptedQty: receiptedQty,
                    ladingItemQty: ladingItemQty,
                    qualityControlPassedQty: qualityControlPassedQty,
                    qualityControlFailedQty: qualityControlFailedQty);
    }
    public CargoItemSummary EditCargoItemSummary(
        CargoItemSummary cargoItemSummary,
        byte[] rowVersion,
        TValue<double> receiptedQty = null,
        TValue<double> ladingItemQty = null,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null)
    {

      if (receiptedQty != null)
        cargoItemSummary.ReceiptedQty = receiptedQty;
      if (ladingItemQty != null)
        cargoItemSummary.LadingItemQty = ladingItemQty;
      if (qualityControlPassedQty != null)
        cargoItemSummary.QualityControlPassedQty = qualityControlPassedQty;
      if (qualityControlFailedQty != null)
        cargoItemSummary.QualityControlFailedQty = qualityControlFailedQty;
      repository.Update(rowVersion: rowVersion, entity: cargoItemSummary);
      return cargoItemSummary;
    }
    #endregion
    #region Delete
    public void DeleteCargoItemSummary(int id)
    {

      var cargoItemSummary = GetCargoItemSummary(id: id);
      repository.Delete(cargoItemSummary);
    }
    #endregion
    #region Reset
    public CargoItemSummary ResetCargoItemSummaryByCargoItemId(int cargoItemId)
    {

      var cargoItemSummary = GetCargoItemSummaryByCargoItemId(cargoItemId: cargoItemId); ; return ResetCargoItemSummary(cargoItemSummary: cargoItemSummary);

    }
    public CargoItemSummary ResetCargoItemSummary(int id)
    {

      var cargoItemSummary = GetCargoItemSummary(id: id); ; return ResetCargoItemSummary(cargoItemSummary: cargoItemSummary);

    }
    public CargoItemSummary ResetCargoItemSummary(CargoItemSummary cargoItemSummary)
    {

      #region Get CargoItems
      var ladingItems = GetLadingItems(
              selector: e =>
                  new
                  {
                    LadingItemQty = (double?)e.Qty,
                    ReceiptedQty = (double?)e.LadingItemSummary.ReceiptedQty,
                    QualityControlPassedQty = (double?)e.LadingItemSummary.QualityControlPassedQty,
                    QualityControlFailedQty = (double?)e.LadingItemSummary.QualityControlFailedQty,
                  },
              isDelete: false,
              cargoItemId: cargoItemSummary.CargoItem.Id);
      var receiptedQty = 0d;
      var ladingItemQty = 0d;
      var qualityControlPassedQty = 0d;
      var qualityControlFailedQty = 0d;
      if (ladingItems.Any())
      {
        receiptedQty = ladingItems.Sum(i => i.ReceiptedQty ?? 0);
        ladingItemQty = ladingItems.Sum(i => i.LadingItemQty ?? 0);
        qualityControlPassedQty = ladingItems.Sum(i => i.QualityControlPassedQty ?? 0);
        qualityControlFailedQty = ladingItems.Sum(i => i.QualityControlFailedQty ?? 0);
      }
      #endregion
      #region EditCargoItemSummary
      var editedCargoItemSummary = EditCargoItemSummary(
              cargoItemSummary: cargoItemSummary,
              rowVersion: cargoItemSummary.RowVersion,
              receiptedQty: receiptedQty,
              ladingItemQty: ladingItemQty,
              qualityControlPassedQty: qualityControlPassedQty,
              qualityControlFailedQty: qualityControlFailedQty);
      #endregion
      #region GetPurchaseOrder
      var purchaseOrder = GetCargoItemSummary(
              selector: e => e.CargoItem.PurchaseOrder,
              id: cargoItemSummary.Id);
      #endregion
      #region Reset PurchaseOrderStatus
      ResetPurchaseOrderStatus(purchaseOrder: purchaseOrder);
      #endregion

      #region Set FinancialTransactionIsPermanent
      if (receiptedQty == editedCargoItemSummary.CargoItem.Qty && qualityControlPassedQty == editedCargoItemSummary.CargoItem.Qty)
      {
        var financialTransactions = editedCargoItemSummary.CargoItem.FinancialTransactionBatch.FinancialTransactions;
        foreach (var financialTransaction in financialTransactions)
        {
          App.Internals.Accounting.SetFinancialTransactionIsPermanent(financialTransaction, true);
        }
      }
      else
      {
        var financialTransactions = editedCargoItemSummary.CargoItem.FinancialTransactionBatch.FinancialTransactions;
        foreach (var financialTransaction in financialTransactions)
        {
          App.Internals.Accounting.SetFinancialTransactionIsPermanent(financialTransaction, false);
        }
      }
      #endregion

      return cargoItemSummary;
    }

    #endregion

  }
}
