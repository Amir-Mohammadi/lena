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
  public class StuffSerialExistInSerialBufferException : InternalServiceException
  {
    public string Serial { get; }
    public double? Qty { get; }
    public string UnitName { get; set; }

    public StuffSerialExistInSerialBufferException(string serial, double? qty, string unitName)
    {
      this.Serial = serial;
      this.Qty = qty;
      this.UnitName = unitName;
    }
  }
}
