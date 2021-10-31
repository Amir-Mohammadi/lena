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
  public class PrinterSettingsIsRequiredForTraceableStuffsException : InternalServiceException
  {
    public int StuffId { get; set; }

    public string StuffName { get; set; }

    public PrinterSettingsIsRequiredForTraceableStuffsException(int stuffId, string stuffName)
    {
      this.StuffId = stuffId;
      StuffName = stuffName;
    }
  }
}
