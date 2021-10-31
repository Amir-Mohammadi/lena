using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using lena.Domains.Enums;
using lena.Models.Printers;

namespace lena.Services.Core.Common
{
  public static class PrinterHelper
  {
    public static IEnumerable<SystemPrinterInfo> GetSystemPrinters()
    {
      var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");

      foreach (var printer in printerQuery.Get())
      {
        var status = printer.GetPropertyValue("Status").ToString();
        var st = PrinterStatus.Unknown;
        if (!Enum.TryParse(status, out st))
          st = PrinterStatus.Unknown;
        var printerInfo = new SystemPrinterInfo
        {
          Name = printer.GetPropertyValue("Name").ToString(),
          IsDefault = Convert.ToBoolean(printer.GetPropertyValue("Default")),
          IsNetworkPrinter = Convert.ToBoolean(printer.GetPropertyValue("Network")),
          SharedName = printer.GetPropertyValue("SharedName")?.ToString() ?? "",
          Status = st
        };


        yield return printerInfo;

      }
    }
  }
}
