using System;
using System.Runtime.InteropServices;
using System.Text;
namespace lena.Services.PrinterManagment
{
  public class ZebraPrinter : BarcodePrinter
  {
    public const string _commandStartSymbol = "^XA";
    public const string _commandEndSymbol = "^XZ";

    private StringBuilder _commandBuilder = new StringBuilder();

    public ZebraPrinter(string name) : base(name)
    {
    }
    public override void Print(BarcodeElement[] barcodes, int columnCount)
    {
      columnCount = columnCount == 0 ? 1 : columnCount;
      var paperWidth = Setting.PaperWidth;

      //Setting.PaperMargin = 0;
      //Setting.HorizontalGap = 0;
      //Setting.LabelWidth = 25;
      //Setting.PrintSpeed = 2; //Zebra print speed is between 2 - 12
      //Setting.ReferencePoint = new System.Drawing.PointF(2, 1);
      _commandBuilder.Clear();
      int j = 0;
      for (int i = 0; i < barcodes.Length; i++)
      {
        var item = barcodes[i];
        item.Info.Size = 1.5F;
        item.Series.Size = 1.5F;
        //item.BarcodeHeight = 5;

        if (item.AutoSetLocation)
        {
          var startPointX = Setting.ReferencePoint.X + Setting.LabelPadding;
          var startpointY = Setting.ReferencePoint.Y + Setting.LabelPadding;
          if (columnCount > 0)
            startPointX = startPointX + ((j + 1) * Setting.HorizontalGap) + (j * Setting.LabelWidth);

          item.Location = new System.Drawing.PointF(startPointX, startpointY);
        }

        SetPrintPosition(item.Location.X, item.Location.Y);
        SetBarcodeCommand(item.BarcodeType, item.FullBarcode, item.BarcodeHeight, 2.5F, 2, item.NarrowRatio);
        var barcodePositionY = item.Location.Y + item.BarcodeHeight;
        if (item.PrintFooterText)
        {
          //SetHorizontalLine(item.Location.X, barcodePositionY + 3F, Setting.LabelWidth + 5);
          SetPrintPosition(item.Location.X, barcodePositionY + 3.5F);
          SetTextPrintCommand(item.Info.Text, Alignment.Left, item.Info.Size, item.Info.Size);
        }
        SetPrintPosition(item.Location.X + 15F, barcodePositionY + 3.5F);
        SetTextPrintCommand(item.Series.Text, Alignment.Left, item.Series.Size, item.Series.Size);

        if ((j + 1) % columnCount == 0)
        {
          SendCommands();
          j = 0;
        }
        else if (i + 1 == barcodes.Length)
          SendCommands();
        else
          j++;
      }
    }
    public override void Print(string text)
    {
      SetTextPrintCommand(text, Alignment.Left, 2, 2);
      SendCommands();
    }

    /// <summary>
    /// Reset to refrence point of paper.
    /// </summary>
    private void ResetPrintPosition()
    {
      _commandBuilder.AppendLine($"^FO{ConvertMilimeterToDot(Setting.ReferencePoint.X)},{ConvertMilimeterToDot(Setting.ReferencePoint.Y)}");
    }

    private void SetBarcodeCommand(string barcodeType, string barcode, float height, float barcodeTextWidth, float barcodeTextHeight, float narrowBarWidth = 1)
    {
      // TODO Support all format of barcodes !:)
      //Currently default is 128b

      //Format   ^BCo,h,f,g,e,m
      // o = orientation Accepted Values: 
      //            N = normal
      //R = rotated 90 degrees(clockwise)
      //I = inverted 180 degrees
      //B = read from bottom up, 270 degrees
      //Default Value: current ^ FW value
      //===================================
      //Format ^Afo,h,w
      //===================================
      //            h = Character Height
      //(in dots)
      //Scalable
      //Accepted Values: 10 to 32000
      //Default Value: last accepted ^ CF
      //Bitmapped
      //Accepted Values: multiples of height from 1 to 10 times the standard
      //height, in increments of 1
      //Default Value: last accepted ^ CF
      //w = width(in dots) Scalable
      //Accepted Values: 10 to 32000
      //Default Value: last accepted ^ CF
      //Bitmapped
      //Accepted Values: multiples of width from 1 to 10 times the standard
      //width, in increments of 1
      //Default Value: last accepted ^ CF

      _commandBuilder.AppendLine($"^BY1^AAN,3,3^BCN,{ConvertMilimeterToDot(height)},Y,N,N,A");
      _commandBuilder.AppendLine($"^A0N,{ConvertMilimeterToDot(barcodeTextHeight)},{ConvertMilimeterToDot(barcodeTextWidth)}^FD{barcode}^FS");
    }
    /// <summary>
    /// toleranceDot اضافی هست 
    /// </summary>
    /// <param name="x">mm</param>
    /// <param name="y">mm</param>
    private void SetPrintPosition(float x, float y)
    {
      _commandBuilder.AppendLine($"^FO{ConvertMilimeterToDot(x)},{ConvertMilimeterToDot(y)}");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="borderThickness"></param>
    /// <param name="lineColor">B: Black or W:White</param>
    /// <param name="degreeOfCornerRounding"></param>
    private void SetHorizontalLine(float x, float y, float width, float borderThickness = 1, string lineColor = "B", float degreeOfCornerRounding = 0)
    {
      int height = 0;
      _commandBuilder.AppendLine($"^FO{ConvertMilimeterToDot(x)},{ConvertMilimeterToDot(y)}^GB{ConvertMilimeterToDot(width)},{ConvertMilimeterToDot(height)},{borderThickness},{lineColor},{degreeOfCornerRounding}^FS");
    }
    private void SetTextPrintCommand(string text, Alignment textAlign, float fontHeight, float fontWidth)
    {
      var alignSymbol = "L";
      switch (textAlign)
      {
        case Alignment.Left:
          alignSymbol = "L";
          break;
        case Alignment.Center:
          alignSymbol = "C";
          break;
        case Alignment.Right:
          alignSymbol = "R";
          break;
        case Alignment.Justify:
          alignSymbol = "J";
          break;
        default:
          alignSymbol = "L";
          break;
      }

      _commandBuilder.AppendLine($"^FB{ConvertMilimeterToDot(Setting.LabelWidth)},1,0,{alignSymbol},0");
      _commandBuilder.AppendLine($"^A0N,{ConvertMilimeterToDot(fontHeight)},{ConvertMilimeterToDot(2)}^FD" + text + "^FS");
    }
    private void SendCommands()
    {
      //_commandBuilder.AppendLine("^PR1,2,A ");
      _commandBuilder.Insert(0, _commandStartSymbol + " \r\n");
      _commandBuilder.AppendLine(_commandEndSymbol);

      var commands = _commandBuilder.ToString();
      SendStringToPrinter(commands);
      _commandBuilder.Clear();
    }
    public override void SetPrinterSetting(PrinterSetting setting)
    {
      //Todo move to setting file or db, get it from user or top level


      //Set Printer Speed
      _commandBuilder.AppendLine($"^PR2,2,2");
      SendCommands();
    }


    #region Api methods
    public bool SendStringToPrinter(string text)
    {
      IntPtr pBytes = Marshal.StringToCoTaskMemAnsi(text);
      SendBytesToPrinter(pBytes, text.Length);
      Marshal.FreeCoTaskMem(pBytes);
      return true;
    }
    public bool SendBytesToPrinter(IntPtr pBytes, int dwCount)
    {

      int num = 0;
      int dwWritten = 0;
      IntPtr hPrinter = new IntPtr(0);
      DOCINFOA di = new DOCINFOA();
      bool flag = false;
      di.pDocName = "Barcode Document";
      di.pDataType = "RAW";
      if (OpenPrinter(this.Name.Normalize(), out hPrinter, IntPtr.Zero))
      {
        if (StartDocPrinter(hPrinter, 1, di))
        {
          if (StartPagePrinter(hPrinter))
          {
            flag = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
            EndPagePrinter(hPrinter);
          }
          EndDocPrinter(hPrinter);
        }
        ClosePrinter(hPrinter);
      }
      if (!flag)
      {
        num = Marshal.GetLastWin32Error();
      }
      return flag;
    }
    public bool SendFileToPrinter(string text)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(text);
      bool flag = false;
      IntPtr destination = new IntPtr(0);
      int length = text.Length;
      destination = Marshal.AllocCoTaskMem(length);
      Marshal.Copy(bytes, 0, destination, length);
      flag = SendBytesToPrinter(destination, length);
      Marshal.FreeCoTaskMem(destination);
      return flag;
    }
    public bool OpenPrinter(string szPrinter, out IntPtr hPrinter, IntPtr pd)
    {
      return ZebraWrapper.OpenPrinter(szPrinter, out hPrinter, pd);
    }
    public bool ClosePrinter(IntPtr hPrinter)
    {
      return ZebraWrapper.ClosePrinter(hPrinter);
    }
    public bool EndDocPrinter(IntPtr hPrinter)
    {
      return ZebraWrapper.EndDocPrinter(hPrinter);
    }
    public bool EndPagePrinter(IntPtr hPrinter)
    {
      return ZebraWrapper.EndPagePrinter(hPrinter);
    }
    public bool StartDocPrinter(IntPtr hPrinter, int level, DOCINFOA di)
    {
      return ZebraWrapper.StartDocPrinter(hPrinter, level, di);
    }
    public bool StartPagePrinter(IntPtr hPrinter)
    {
      return ZebraWrapper.StartPagePrinter(hPrinter);
    }
    public bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten)
    {
      return ZebraWrapper.WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
    }


    #endregion

  }

  #region DLL Import / Wrapper
  public class ZebraWrapper
  {
    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true, ExactSpelling = true)]
    public static extern bool ClosePrinter(IntPtr hPrinter);
    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true, ExactSpelling = true)]
    public static extern bool EndDocPrinter(IntPtr hPrinter);
    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true, ExactSpelling = true)]
    public static extern bool EndPagePrinter(IntPtr hPrinter);
    [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);
    [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);
    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true, ExactSpelling = true)]
    public static extern bool StartPagePrinter(IntPtr hPrinter);
    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true, ExactSpelling = true)]
    public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

  }

  [StructLayout(LayoutKind.Sequential)]
  public class DOCINFOA
  {
    [MarshalAs(UnmanagedType.LPStr)]
    public string pDocName;
    [MarshalAs(UnmanagedType.LPStr)]
    public string pOutputFile;
    [MarshalAs(UnmanagedType.LPStr)]
    public string pDataType;
  }

  #endregion
}
