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
  public class CargoItemAndStoreReceiptUnitTypeDoNotMatchException : InternalServiceException
  {
    public string CargoItemCode { get; set; }
    public string CargoItemUnitName { get; set; }
    public string CargoItemUnitTypeName { get; set; }

    public string StoreReceiptCode { get; set; }
    public string StoreReceiptUnitName { get; set; }
    public string StoreReceiptUnitTypeName { get; set; }

    public CargoItemAndStoreReceiptUnitTypeDoNotMatchException(
        string cargoItemCode,
        string cargoItemUnitName,
        string cargoItemUnitTypeName,
        string storeReceiptCode,
        string storeReceiptUnitName,
        string storeReceiptUnitTypeName)
    {
      CargoItemCode = cargoItemCode;
      CargoItemUnitName = cargoItemUnitName;
      CargoItemUnitTypeName = cargoItemUnitTypeName;
      StoreReceiptCode = storeReceiptCode;
      StoreReceiptUnitName = storeReceiptUnitName;
      StoreReceiptUnitTypeName = storeReceiptUnitTypeName;
    }
  }
}
