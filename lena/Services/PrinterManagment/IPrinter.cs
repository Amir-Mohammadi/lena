using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace lena.Services.PrinterManagment
{
  public interface IPrinter
  {
    void Print(string text);
  }
  public interface IBarcodePrinter : IPrinter
  {
    PrinterSetting Setting { get; set; }
    void Initialize();
    void Print(BarcodeElement[] barcodes, int columnCount);
  }
}