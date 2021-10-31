using lena.Services.Core.Foundation;
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
  public class SerialIsInAnOtherWarehouseException : InternalServiceException
  {
    public string Serial { get; set; }
    public string WarehouseName { get; }

    public SerialIsInAnOtherWarehouseException(string serial, string warehouseName)
    {
      Serial = serial;
      WarehouseName = warehouseName;
    }
  }
}
