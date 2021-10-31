using System;
using lena.Models.Common;
using lena.Domains;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Domains.Enums;
using lena.Models.ApplicationBase.Unit;
using lena.Models.QualityControl.QualityControlItem;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {

    #region AddProcess
    public ProductionQualityControl AddProductionQualityControlProcess(
        ProductionQualityControl productionQualityControl,
        TransactionBatch transactionBatch,
        int stuffId,
        short warehouseId,
        string description,
        AddQualityControlItemTransactionInput[] addQualityControlItemTransactionInputs
        )
    {


      productionQualityControl = productionQualityControl ?? repository.Create<ProductionQualityControl>();
      productionQualityControl.QualityControlType = QualityControlType.ProductionQualityControl;
      AddQualityControlProcess(
                    qualityControl: productionQualityControl,
                    transactionBatch: transactionBatch,
                    stuffId: stuffId,
                    warehouseId: warehouseId,
                    description: description,
                    addQualityControlItemTransactionInputs: addQualityControlItemTransactionInputs);
      return productionQualityControl;
    }
    #endregion

    #region Get
    public ProductionQualityControl GetProductionQualityControl(int id) => GetProductionQualityControl(selector: e => e, id: id);
    public TResult GetProductionQualityControl<TResult>(
        Expression<Func<ProductionQualityControl, TResult>> selector,
        int id)
    {

      var productionQualityControl = GetProductionQualityControls(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (productionQualityControl == null)
        throw new ProductionQualityControlNotFoundException(id);
      return productionQualityControl;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionQualityControls<TResult>(
        Expression<Func<ProductionQualityControl, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<QualityControlStatus> status = null,
        TValue<int> stuffId = null,
        TValue<QualityControlType> qualityControlType = null,
        TValue<int> warehouseId = null,
        TValue<int> unitId = null,
        TValue<double> qty = null,
        TValue<double> acceptedQty = null,
        TValue<double> failedQty = null,
        TValue<double> conditionalRequestQty = null,
        TValue<double> conditionalQty = null,
        TValue<double> returnedQty = null,
        TValue<double> consumedQty = null,
        TValue<int> storeReceiptId = null)
    {

      var baseQuery = GetQualityControls(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description,
                    status: status,
                    stuffId: stuffId,
                    qualityControlType: qualityControlType,
                    warehouseId: warehouseId,
                    unitId: unitId,
                    qty: qty,
                    acceptedQty: acceptedQty,
                    failedQty: failedQty,
                    conditionalRequestQty: conditionalRequestQty,
                    conditionalQty: conditionalQty,
                    returnedQty: returnedQty,
                    consumedQty: consumedQty);
      var query = baseQuery.OfType<ProductionQualityControl>();
      return query.Select(selector);
    }
    #endregion
  }
}
