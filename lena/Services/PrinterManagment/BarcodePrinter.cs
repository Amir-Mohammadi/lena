using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.PrinterManagment
{
  public abstract class BarcodePrinter : Printer, IBarcodePrinter
  {
    protected BarcodePrinter(string name)
    {

      Name = name;
      Setting = new PrinterSetting();
    }
    public virtual PrinterSetting Setting { get; set; }
    //public virtual PrinterFont Font { get; set; }

    public virtual void Initialize()
    {
      SetPrinterSetting(Setting);
    }
    protected int ConvertMilimeterToDot(float value)
    {
      //200 DPI: 1 mm = 8 dots
      //300 DPI: 1 mm = 12 dots

      return (int)(value * (Setting.PrintDpi <= 203 ? 8 : 12));
    }
    public abstract void SetPrinterSetting(PrinterSetting setting);
    public abstract void Print(BarcodeElement[] barcodes, int columnCount);
  }
}
