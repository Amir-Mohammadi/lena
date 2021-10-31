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
    #region Get By PurchaseOrderId
    public PurchaseOrderSummary GetPurchaseOrderSummaryByPurchaseOrderId(int purchaseOrderId) =>
    GetPurchaseOrderSummaryByPurchaseOrderId(selector: e => e, purchaseOrderId: purchaseOrderId);
    public TResult GetPurchaseOrderSummaryByPurchaseOrderId<TResult>(
        Expression<Func<PurchaseOrderSummary, TResult>> selector,
        int purchaseOrderId)
    {

      var purchaseOrderSummary = GetPurchaseOrderSummarys(
                    selector: selector,
                    purchaseOrderId: purchaseOrderId)


                .FirstOrDefault();
      if (purchaseOrderSummary == null)
        throw new PurchaseOrderSummaryForPurchaseOrderNotFoundException(purchaseOrderId: purchaseOrderId);
      return purchaseOrderSummary;
    }
    #endregion
    #region Get
    public PurchaseOrderSummary GetPurchaseOrderSummary(int id) => GetPurchaseOrderSummary(selector: e => e, id: id);
    public TResult GetPurchaseOrderSummary<TResult>(
        Expression<Func<PurchaseOrderSummary, TResult>> selector,
        int id)
    {

      var purchaseOrderSummary = GetPurchaseOrderSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (purchaseOrderSummary == null)
        throw new PurchaseOrderSummaryNotFoundException(id: id);
      return purchaseOrderSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetPurchaseOrderSummarys<TResult>(
            Expression<Func<PurchaseOrderSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> cargoedQty = null,
            TValue<double> receiptedQty = null,
            TValue<double> qualityControlPassedQty = null,
            TValue<double> qualityControlFailedQty = null,
            TValue<int> purchaseOrderId = null)
    {

      var query = repository.GetQuery<PurchaseOrderSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (cargoedQty != null)
        query = query.Where(x => x.CargoedQty == cargoedQty);
      if (receiptedQty != null)
        query = query.Where(x => x.ReceiptedQty == receiptedQty);
      if (qualityControlPassedQty != null)
        query = query.Where(x => x.QualityControlPassedQty == qualityControlPassedQty);
      if (qualityControlFailedQty != null)
        query = query.Where(x => x.QualityControlFailedQty == qualityControlFailedQty);
      if (purchaseOrderId != null)
        query = query.Where(x => x.PurchaseOrder.Id == purchaseOrderId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public PurchaseOrderSummary AddPurchaseOrderSummary(
            double cargoedQty,
            double receiptedQty,
            double qualityControlPassedQty,
            double qualityControlFailedQty,
            int purchaseOrderId)
    {

      var purchaseOrderSummary = repository.Create<PurchaseOrderSummary>();
      purchaseOrderSummary.CargoedQty = cargoedQty;
      purchaseOrderSummary.ReceiptedQty = receiptedQty;
      purchaseOrderSummary.QualityControlPassedQty = qualityControlPassedQty;
      purchaseOrderSummary.QualityControlFailedQty = qualityControlFailedQty;
      purchaseOrderSummary.PurchaseOrder = GetPurchaseOrder(id: purchaseOrderId);
      repository.Add(purchaseOrderSummary);
      return purchaseOrderSummary;
    }
    #endregion
    #region Edit

    public PurchaseOrderSummary EditPurchaseOrderSummary(
        int id,
        byte[] rowVersion,
        TValue<double> cargoedQty = null,
        TValue<double> receiptedQty = null,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null)
    {

      var purchaseOrderSummary = GetPurchaseOrderSummary(id: id);
      return EditPurchaseOrderSummary(
                    purchaseOrderSummary: purchaseOrderSummary,
                    rowVersion: rowVersion,
                    cargoedQty: cargoedQty,
                    receiptedQty: receiptedQty,
                    qualityControlPassedQty: qualityControlPassedQty,
                    qualityControlFailedQty: qualityControlFailedQty);
    }

    public PurchaseOrderSummary EditPurchaseOrderSummary(
        PurchaseOrderSummary purchaseOrderSummary,
        byte[] rowVersion,
        TValue<double> cargoedQty = null,
        TValue<double> receiptedQty = null,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null)
    {


      if (cargoedQty != null)
        purchaseOrderSummary.CargoedQty = cargoedQty;
      if (receiptedQty != null)
        purchaseOrderSummary.ReceiptedQty = receiptedQty;
      if (qualityControlPassedQty != null)
        purchaseOrderSummary.QualityControlPassedQty = qualityControlPassedQty;
      if (qualityControlFailedQty != null)
        purchaseOrderSummary.QualityControlFailedQty = qualityControlFailedQty;
      repository.Update(rowVersion: rowVersion, entity: purchaseOrderSummary);
      return purchaseOrderSummary;
    }

    #endregion
    #region Delete
    public void DeletePurchaseOrderSummary(int id)
    {

      var purchaseOrderSummary = GetPurchaseOrderSummary(id: id);
      repository.Delete(purchaseOrderSummary);
    }
    #endregion
    #region Reset
    public PurchaseOrderSummary ResetPurchaseOrderSummaryByPurchaseOrderId(int purchaseOrderId)
    {

      var purchaseOrderSummary = GetPurchaseOrderSummaryByPurchaseOrderId(purchaseOrderId: purchaseOrderId); ; return ResetPurchaseOrderSummary(purchaseOrderSummary: purchaseOrderSummary);

    }
    public PurchaseOrderSummary ResetPurchaseOrderSummary(int id)
    {

      var purchaseOrderSummary = GetPurchaseOrderSummary(id: id); ; return ResetPurchaseOrderSummary(purchaseOrderSummary: purchaseOrderSummary);

    }
    public PurchaseOrderSummary ResetPurchaseOrderSummary(PurchaseOrderSummary purchaseOrderSummary)
    {

      #region Get PurchaseOrders
      var purchaseOrderQtys = GetPurchaseOrderDetails(
              selector: e =>
                  new
                  {
                    CargoedQty = (Nullable<double>)e.PurchaseOrderDetailSummary.CargoedQty * e.Unit.ConversionRatio / e.PurchaseOrder.Unit.ConversionRatio,
                    ReceiptedQty = (Nullable<double>)e.PurchaseOrderDetailSummary.ReceiptedQty * e.Unit.ConversionRatio / e.PurchaseOrder.Unit.ConversionRatio,
                    QualityControlPassedQty = (Nullable<double>)e.PurchaseOrderDetailSummary.QualityControlPassedQty * e.Unit.ConversionRatio / e.PurchaseOrder.Unit.ConversionRatio,
                    qualityControlFailedQty = (Nullable<double>)e.PurchaseOrderDetailSummary.QualityControlFailedQty * e.Unit.ConversionRatio / e.PurchaseOrder.Unit.ConversionRatio
                  },
              isDelete: false,
              purchaseOrderId: purchaseOrderSummary.PurchaseOrder.Id);
      Nullable<double> cargoedQty = 0d;
      Nullable<double> receiptedQty = 0d;
      Nullable<double> qualityControlPassedQty = 0d;
      Nullable<double> qualityControlFailedQty = 0d;
      if (purchaseOrderQtys.Any())
      {
        cargoedQty = purchaseOrderQtys.Sum(i => i.CargoedQty);
        receiptedQty = purchaseOrderQtys.Sum(i => i.ReceiptedQty);
        qualityControlPassedQty = purchaseOrderQtys.Sum(i => i.QualityControlPassedQty);
        qualityControlFailedQty = purchaseOrderQtys.Sum(i => i.qualityControlFailedQty);

      }
      #endregion
      #region EditPurchaseOrderSummary
      var editedPurchaseOrderSummary = EditPurchaseOrderSummary(
              purchaseOrderSummary: purchaseOrderSummary,
              rowVersion: purchaseOrderSummary.RowVersion,
              cargoedQty: cargoedQty,
              receiptedQty: receiptedQty,
              qualityControlPassedQty: qualityControlPassedQty,
              qualityControlFailedQty: qualityControlFailedQty);
      #endregion

      #region Set FinancialTransactionIsPermanent
      if (receiptedQty == editedPurchaseOrderSummary.PurchaseOrder.Qty && qualityControlPassedQty == editedPurchaseOrderSummary.PurchaseOrder.Qty)
      {
        var financialTransactions = editedPurchaseOrderSummary.PurchaseOrder.FinancialTransactionBatch.FinancialTransactions;
        foreach (var financialTransaction in financialTransactions)
        {
          App.Internals.Accounting.SetFinancialTransactionIsPermanent(financialTransaction, true);
        }
      }
      else
      {
        var financialTransactions = editedPurchaseOrderSummary.PurchaseOrder.FinancialTransactionBatch.FinancialTransactions;
        foreach (var financialTransaction in financialTransactions)
        {
          App.Internals.Accounting.SetFinancialTransactionIsPermanent(financialTransaction, false);
        }
      }
      #endregion

      return purchaseOrderSummary;
    }

    #endregion
  }
}
