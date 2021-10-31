using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class PurchaseOrderItemAndStoreReceiptUnitTypeDoNotMatchException : InternalServiceException
  {
    public string PurchaseOrderItemCode { get; set; }
    public string PurchaseOrderItemUnitName { get; set; }
    public string PurchaseOrderItemUnitTypeName { get; set; }

    public string StoreReceiptCode { get; set; }
    public string StoreReceiptUnitName { get; set; }
    public string StoreReceiptUnitTypeName { get; set; }

    public PurchaseOrderItemAndStoreReceiptUnitTypeDoNotMatchException(
        string purchaseOrderItemCode,
        string purchaseOrderItemUnitName,
        string purchaseOrderItemUnitTypeName,
        string storeReceiptCode,
        string storeReceiptUnitName,
        string storeReceiptUnitTypeName)
    {
      PurchaseOrderItemCode = purchaseOrderItemCode;
      PurchaseOrderItemUnitName = purchaseOrderItemUnitName;
      PurchaseOrderItemUnitTypeName = purchaseOrderItemUnitTypeName;
      StoreReceiptCode = storeReceiptCode;
      StoreReceiptUnitName = storeReceiptUnitName;
      StoreReceiptUnitTypeName = storeReceiptUnitTypeName;
    }
  }
}
