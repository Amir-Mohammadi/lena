using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public ReturnedExitReceiptRequest AddReturnedExitReceiptRequestProcess(
        ReturnedExitReceiptRequest returnedExitReceiptRequest,
        TransactionBatch transactionBatch,
        int returnStoreReceiptId,
        string description,
        short warehouseId,
        double qty,
        byte unitId,
        int exitReceiptRequestTypeId,
        int stuffId,
        int? billOfMaterialVersion,
        string address,
        int cooperatorId)
    {

      returnedExitReceiptRequest = returnedExitReceiptRequest ?? repository.Create<ReturnedExitReceiptRequest>();
      returnedExitReceiptRequest.ReturnStoreReceiptId = returnStoreReceiptId;
      App.Internals.SaleManagement.AddExitReceiptRequestProcess(
                    exitReceiptRequest: returnedExitReceiptRequest,
                    description: description,
                    warehouseId: warehouseId,
                    qty: qty,
                    unitId: unitId,
                    exitReceiptRequestTypeId: exitReceiptRequestTypeId,
                    stuffId: stuffId,
                    address: address,
                    cooperatorId: cooperatorId,
                    serials: null);
      return returnedExitReceiptRequest;
    }
    #endregion
  }
}