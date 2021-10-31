using lena.Services.Core.Foundation;
using lena.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class ResetStoreReceiptTypeNotSupportedException : InternalServiceException
  {
    public int StoreReceiptId { get; set; }
    public StoreReceiptType StoreReceiptType { get; set; }

    public ResetStoreReceiptTypeNotSupportedException(int storeReceiptId, StoreReceiptType type)
    {
      StoreReceiptId = storeReceiptId;
      StoreReceiptType = type;
    }
  }
}
