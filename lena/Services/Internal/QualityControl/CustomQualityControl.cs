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
    public CustomQualityControl AddCustomQualityControlProcess(
        CustomQualityControl customQualityControl,
        TransactionBatch transactionBatch,
        int stuffId,
        short warehouseId,
        string description,
        AddQualityControlItemTransactionInput[] addQualityControlItemTransactionInputs)
    {

      customQualityControl = customQualityControl ?? repository.Create<CustomQualityControl>();
      customQualityControl.QualityControlType = QualityControlType.CustomQualityControl;
      AddQualityControlProcess(
                    qualityControl: customQualityControl,
                    transactionBatch: transactionBatch,
                    stuffId: stuffId,
                    warehouseId: warehouseId,
                    description: description,
                    addQualityControlItemTransactionInputs: addQualityControlItemTransactionInputs);
      return customQualityControl;
    }
    #endregion

    #region Get
    public CustomQualityControl GetCustomQualityControl(int id) => GetCustomQualityControl(selector: e => e, id: id);
    public TResult GetCustomQualityControl<TResult>(
        Expression<Func<CustomQualityControl, TResult>> selector,
        int id)
    {

      var customQualityControl = GetCustomQualityControls(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (customQualityControl == null)
        throw new CustomQualityControlNotFoundException(id);
      return customQualityControl;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetCustomQualityControls<TResult>(
        Expression<Func<CustomQualityControl, TResult>> selector,
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
        TValue<int> storeReceiptId = null,
        TValue<string[]> serials = null)
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
                    consumedQty: consumedQty,
                    serials: serials);
      var query = baseQuery.OfType<CustomQualityControl>();
      return query.Select(selector);
    }
    #endregion
  }
}
