using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;


namespace lena.Services.Common.Helpers
{
  public static class PrinterHelper
  {
    public static string CheckPrinterConfiguration(string printerName, string networkAddress)
    {
      var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters;

      var printerArray = new string[printers.Count];
      printers.CopyTo(printerArray, 0);

      //var printers = GetAllInstalledPrinters().Select(x => x.Name).ToArray();


      //throw new Exception(string.Join("|" , printerArray));


      //var name = !string.IsNullOrEmpty(networkAddress) ? networkAddress : printerName;

      return printerArray.FirstOrDefault(p => p == printerName);

    }

    public static List<PrinterInfo> GetAllInstalledPrinters()
    {
      var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
      var printerList = new List<PrinterInfo>();

      foreach (var printer in printerQuery.Get())
      {
        printerList.Add(new PrinterInfo()
        {
          Name = printer.GetPropertyValue("Name").ToString(),
          Status = printer.GetPropertyValue("Status").ToString(),
          IsDefault = Convert.ToBoolean(printer.GetPropertyValue("Default")),
          IsNetworkPrinter = Convert.ToBoolean(printer.GetPropertyValue("Network"))
        });
      }

      return printerList;
    }

  }

  public class PrinterInfo
  {
    public string Name { get; set; }
    public string Status { get; set; }
    public bool IsDefault { get; set; }
    public bool IsNetworkPrinter { get; set; }
  }
}
