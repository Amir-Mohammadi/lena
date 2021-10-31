using lena.Services.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.PrinterManagment
{
  public class TSCPrinter : BarcodePrinter
  {
    public TSCPrinter(string name) : base(name)
    {
    }

    public override void Initialize()
    {
      base.Initialize();

      SetReferencePoint(Setting.ReferencePoint.X, Setting.ReferencePoint.Y);
      SetPrintDirection(PrintDirection.LeftUpperCorner);
      SetLabelSize(Setting.PaperWidth, Setting.LabelHeight);
      SetLabelGap(Setting.VerticalGap, 0);
    }
    public override void Print(string text)
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="barcodes"></param>
    /// <param name="columnCount"></param>
    public override void Print(BarcodeElement[] barcodes, int columnCount)
    {
      OpenPort();

      ClearBuffer();

      columnCount = columnCount == 0 ? 1 : columnCount;
      var paperWidth = Setting.PaperWidth;

      int j = 0;
      for (int i = 0; i < barcodes.Length; i++)
      {
        //اضافه کردن تعداد ستون های چاپ

        //Program.TSCLIB_DLL.barcode(x, y, BarcodeType, (int.Parse(BarcodeHeight) / 0.125).ToString(),
        //    DisPlayBarcode, RotateBarcode, Continuity, WidthBarcode, barcode);


        var item = barcodes[i];
        if (item.AutoSetLocation)
        {
          var startPointX = Setting.LabelPadding;
          var startpointY = Setting.LabelPadding;
          if (columnCount > 0)
            startPointX = (j) * (Setting.LabelWidth + Setting.HorizontalGap);

          item.Location = new System.Drawing.Point(startPointX, startpointY);

        }
        //SetBarcodePrintSetting(item);
        SetBarcode(item);
        item.Info.Location = new System.Drawing.PointF(item.Location.X, item.BarcodeHeight + 2.5F);
        if (item.PrintFooterText)
          SetPrinterFont(item.Info);

        item.Series.Location = new System.Drawing.PointF(item.Location.X + 16f, item.BarcodeHeight + 2.5F);
        SetPrinterFont(item.Series);
        //xTxt = (int.Parse(x) + int.Parse(LengthText)).ToString();
        //yTxt = (int.Parse(y) + int.Parse(WidthText)).ToString();
        //Program.TSCLIB_DLL.printerfont(xTxt, yTxt, "1", RotateTxt, "1", "1", NameTxt + " " + q.OrderNumber);
        //SetPrinterFont(new PrinterFont()
        //{
        //    Text = "paralr.ir",//OrderNumber
        //    FontTypeName = "128",

        //});
        if ((j + 1) % columnCount == 0)
        {
          PrintLabel(1, 1);
          j = 0;
          ClearBuffer();
        }
        else if (i + 1 == barcodes.Length)
        {
          PrintLabel(1, 1);
        }
        else
          j++;

      }
      ClearBuffer();



      ClosePort();
    }

    public void OpenPort()
    {
      OpenPort(string.IsNullOrEmpty(NetworkAddress) ? Name : Name);
      Initialize();
    }
    /// <summary>
    /// Open printer port
    ///(1)For local printer, please specify the printer driver name.Like: “TTP-244 Plus”
    ///(2)For network printer, please specify the UNC path and printer name.Like:“\\server\TTP243”
    ///(3)For centronics interface directly, please specify LPT1toLPT4.Like: “LPT1”
    ///(4)For USB interface directly, please specify USB.Like: “USB”
    /// </summary>
    public void OpenPort(string printerName)
    {
      TSCWrapper.OpenPort(printerName);
    }
    /// <summary>
    /// Close Windows printer spool.
    /// </summary>
    public void ClosePort()
    {
      TSCWrapper.ClosePort();
    }
    /// <summary>
    ///Clear
    /// </summary>
    public void ClearBuffer()
    {
      TSCWrapper.ClearBuffer();
    }
    /// <summary>
    /// Defines the gap distance between two labels (0, 0 Continuous label).
    /// </summary>
    /// <param name="m">The gap distance between two labels
    ///0 ≤ m ≤1 (inch), 0 ≤ m ≤ 25.4 (mm)
    ///0 ≤ m ≤5 (inch), 0 ≤ m ≤ 127 (mm) / since V6.21 EZ and later firmware</param>
    /// <param name="n">The offset distance of the gap n ≤ label length(inch or mm)</param>
    /// <param name="unit"></param>
    /// <returns></returns>
    public int SetLabelGap(float m, float n, MeasurementUnit unit = MeasurementUnit.Milimeter)
    {
      var unitTxt = GetMeasurementUnitText(unit);
      return TSCWrapper.SendCommand($"GAP {m} {unitTxt},{n} {unitTxt}");
    }
    /// <summary>
    /// This command defines the label width and length
    /// </summary>
    /// <returns></returns>
    public int SetLabelSize(float width, float length, MeasurementUnit unit = MeasurementUnit.Milimeter)
    {
      var unitTxt = GetMeasurementUnitText(unit);
      return TSCWrapper.SendCommand($"SIZE {width} {unitTxt},{length} {unitTxt}");
    }

    public int SetPrintDirection(PrintDirection direction)
    {
      var dir = (direction == PrintDirection.LeftUpperCorner ? 1 : 0);
      return TSCWrapper.SendCommand($"DIRECTION {dir},0");
    }
    /// <summary>
    /// This command defines the reference point of the label. The reference (origin) point varies with the print direction
    /// </summary>
    /// <param name="x">Horizontal coordinate (in dots)</param>
    /// <param name="y">Vertical coordinate (in dots)</param>
    /// <returns></returns>
    public int SetReferencePoint(float x, float y)
    {
      return TSCWrapper.SendCommand($"REFERENCE {(ConvertMilimeterToDot(x + Setting.PaperMargin))},{ConvertMilimeterToDot(y)}");
    }
    public override void SetPrinterSetting(PrinterSetting setting)
    {
      TSCWrapper.Setup(setting.LabelWidth.ToString(), setting.LabelHeight.ToString(), setting.PrintSpeed.ToString(),
          setting.PrintDensity.ToString(), setting.SensorType.ToString(), setting.VerticalGap.ToString(),
          setting.ShiftDistance.ToString());
    }
    public void SetPrinterFont(BarcodeInfo setting)
    {
      TSCWrapper.printerfont(ConvertMilimeterToDot(setting.Location.X).ToString(), ConvertMilimeterToDot(setting.Location.Y).ToString(),
          setting.FontTypeName, setting.Rotation.ToString(), setting.MagnificationRateX.ToString(),
          setting.MagnificationRateY.ToString(), setting.Text);
    }
    /// <summary>
    /// Use built-in bar code formats to print
    /// </summary>
    /// <param name="setting"></param>
    public void SetBarcodePrintSetting(BarcodeElement setting)
    {
      TSCWrapper.Barcode(ConvertMilimeterToDot(setting.Location.X).ToString(), ConvertMilimeterToDot(setting.Location.Y).ToString(),
          setting.BarcodeType, ConvertMilimeterToDot(setting.BarcodeHeight).ToString(), ((int)setting.TextAlignment).ToString(),
          setting.Rotation.ToString(), setting.NarrowRatio.ToString(), setting.WideRatio.ToString(),
          setting.FullBarcode);
    }

    public int SetBarcode(BarcodeElement barcode)
    {
      //BARCODE X, Y,”code type”, height, human readable,rotation,narrow,wide,[alignment,]”content “
      return SendCommand(
          $"BARCODE {ConvertMilimeterToDot(barcode.Location.X)},{ConvertMilimeterToDot(barcode.Location.Y)}," +
          $"\"{barcode.BarcodeType}\",{ConvertMilimeterToDot(barcode.BarcodeHeight)},{(int)barcode.TextAlignment}," +
          $"{barcode.Rotation},{barcode.NarrowRatio},{barcode.WideRatio}," +
          $"{(int)barcode.Alignment},\"{barcode.FullBarcode}\"");

    }
    /// <summary>
    ///Print label content
    /// </summary>
    /// <param name="labelSetCount">Sets up the number of label sets</param>
    /// <param name="numberOfCopies">Sets up the number of print copies</param>
    public void PrintLabel(int labelSetCount, int numberOfCopies)
    {
      TSCWrapper.printlabel(labelSetCount.ToString(), numberOfCopies.ToString());
    }
    /// <summary>
    /// Download mono PCX graphic files to the printer
    /// </summary>
    /// <param name="fileName">File name (including file retrieval path)</param>
    /// <param name="imageName">Names of files that are to be downloaded in the printer memory(Please use capital letters)</param>
    public void DownloadPcxFile(string fileName, string imageName)
    {
      TSCWrapper.DownloadPcx(fileName, imageName);
    }
    /// <summary>
    /// Skip to next page (of label); this function is to be used after setup
    /// </summary>
    public int SkipToNextPage()
    {
      return TSCWrapper.FormFeed();
    }
    /// <summary>
    ///Disable the backfeed function
    /// </summary>
    /// <returns></returns>
    public int DisableBackFeed()
    {
      return TSCWrapper.NobackFeed();
    }
    /// <summary>
    /// Use Windows font to print text.
    /// </summary>
    public void SetWindowsFontSetting(WindowsFont setting)
    {
      TSCWrapper.WindowsFont(setting.Location.X, setting.Location.Y,
          setting.FontHeight, setting.Rotation, setting.FontStyle,
          setting.FontWithUnderline, setting.FontFaceType, setting.Text
          );
    }
    /// <summary>
    ///Display the DLL version on the screen.
    /// </summary>
    /// <returns></returns>
    public int GetPrinterInfo()
    {
      return TSCWrapper.About();
    }

    public int SendCommand(string command)
    {
      return TSCWrapper.SendCommand(command);
    }
    private string GetMeasurementUnitText(MeasurementUnit unit)
    {
      var txt = "";
      switch (unit)
      {
        case MeasurementUnit.Inch:
          txt = "";
          break;
        case MeasurementUnit.Milimeter:
          txt = "mm";
          break;
        case MeasurementUnit.Dot:
          txt = "dot";
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
      }

      return txt;
    }
  }

  public class TSCWrapper
  {
    [DllImport("TSCLIB.dll", EntryPoint = "about")]
    public static extern int About();
    [DllImport("TSCLIB.dll", EntryPoint = "openport")]
    public static extern int OpenPort(string printername);
    [DllImport("TSCLIB.dll", EntryPoint = "barcode")]
    public static extern int Barcode(string x, string y, string type, string height, string readable, string rotation, string narrow, string wide, string code);
    [DllImport("TSCLIB.dll", EntryPoint = "clearbuffer")]
    public static extern int ClearBuffer();
    [DllImport("TSCLIB.dll", EntryPoint = "closeport")]
    public static extern int ClosePort();
    [DllImport("TSCLIB.dll", EntryPoint = "downloadpcx")]
    public static extern int DownloadPcx(string filename, string image_name);
    [DllImport("TSCLIB.dll", EntryPoint = "formfeed")]
    public static extern int FormFeed();
    [DllImport("TSCLIB.dll", EntryPoint = "nobackfeed")]
    public static extern int NobackFeed();
    [DllImport("TSCLIB.dll", EntryPoint = "printerfont")]
    public static extern int printerfont(string x, string y, string fonttype, string rotation, string xmul, string ymul, string text);
    [DllImport("TSCLIB.dll", EntryPoint = "printlabel")]
    public static extern int printlabel(string set, string copy);
    [DllImport("TSCLIB.dll", EntryPoint = "sendcommand")]
    public static extern int SendCommand(string printercommand);
    [DllImport("TSCLIB.dll", EntryPoint = "setup")]
    public static extern int Setup(string labelWidth, string labelHeight, string printSpeed, string density, string sensor, string vertical, string offset);
    [DllImport("TSCLIB.dll", EntryPoint = "windowsfont")]
    public static extern int WindowsFont(int x, int y, int fontheight, int rotation, int fontstyle, int fontunderline, string szFaceName, string content);

  }
}