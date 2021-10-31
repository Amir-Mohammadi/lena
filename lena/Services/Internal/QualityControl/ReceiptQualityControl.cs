using System;
using lena.Models.Common;
using lena.Domains;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.QualityControl.QualityControl;
using lena.Models.QualityControl.QualityControlItem;
using lena.Models.QualityControl.QualityControlTest;
using System.Security.Cryptography;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {

    #region AddProcess
    public ReceiptQualityControl AddReceiptQualityControlProcess(
        ReceiptQualityControl receiptQualityControl,
        TransactionBatch transactionBatch,
        int storeReceiptId,
        int stuffId,
        short warehouseId,
        string description,
        AddQualityControlItemTransactionInput[] addQualityControlItemTransactionInputs,
        TransactionType transactionType = null
        )
    {

      receiptQualityControl = receiptQualityControl ?? repository.Create<ReceiptQualityControl>();
      receiptQualityControl.StoreReceiptId = storeReceiptId;
      receiptQualityControl.QualityControlType = QualityControlType.ReceiptQualityControl;
      AddQualityControlProcess(
                    qualityControl: receiptQualityControl,
                    transactionBatch: transactionBatch,
                    stuffId: stuffId,
                    warehouseId: warehouseId,
                    description: description,
                    addQualityControlItemTransactionInputs: addQualityControlItemTransactionInputs);
      return receiptQualityControl;
    }
    #endregion

    #region Get
    public ReceiptQualityControl GetReceiptQualityControl(int id) => GetReceiptQualityControl(selector: e => e, id: id);
    public TResult GetReceiptQualityControl<TResult>(
        Expression<Func<ReceiptQualityControl, TResult>> selector,
        int id)
    {

      var receiptQualityControl = GetReceiptQualityControls(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (receiptQualityControl == null)
        throw new ReceiptQualityControlNotFoundException(id);
      return receiptQualityControl;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetReceiptQualityControls<TResult>(
        Expression<Func<ReceiptQualityControl, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> qualityControlDepartmentId = null,
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
        TValue<int> receiptId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null)
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
                    qualityControlDepartmentId: qualityControlDepartmentId);
      var query = baseQuery.OfType<ReceiptQualityControl>();
      if (storeReceiptId != null)
        query = query.Where(i => i.StoreReceiptId == storeReceiptId);
      if (receiptId != null)
        query = query.Where(i => i.StoreReceipt.ReceiptId == receiptId);

      if (fromDateTime != null)
        query = query.Where(i => i.StoreReceipt.DateTime >= fromDateTime);

      if (toDateTime != null)
        query = query.Where(i => i.StoreReceipt.DateTime <= toDateTime);
      return query.Select(selector);
    }

    public IQueryable<TResult> GetReceiptQualityControlItems<TResult>(
        Expression<Func<QualityControlItem, TResult>> selector,
        TValue<int> storeReceiptId = null,
          TValue<int> receiptId = null)
    {

      var qcItems = GetQualityControlItems(selector: e => e);

      var receiptQuery = GetReceiptQualityControls(e => e);
      if (storeReceiptId != null)
        receiptQuery = receiptQuery.Where(i => i.StoreReceiptId == storeReceiptId);
      if (receiptId != null)
        receiptQuery = receiptQuery.Where(i => i.StoreReceipt.ReceiptId == receiptId);

      var query = (from item in qcItems
                   join receipt in receiptQuery on item.QualityControlId equals receipt.Id
                   select item);

      return query.Select(selector);
    }
    #endregion

  }
}
