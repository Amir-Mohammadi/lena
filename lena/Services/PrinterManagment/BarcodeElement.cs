using lena.Services.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.PrinterManagment
{
  public class BarcodeElement
  {
    public BarcodeElement()
    {
      Info = new BarcodeInfo();
      Series = new BarcodeInfo();
    }
    /// <summary>
    ///The starting point of the barcode along the X , Y direction, given in points(of 200 DPI, 1 point= 1 / 8 mm; of 300 DPI, 1point=1/12 mm)
    /// </summary>
    public virtual PointF Location { get; set; }
    public BarcodeInfo Info { get; set; }
    public bool AutoSetLocation { get; set; }
    public bool PrintFooterText { get; set; }
    /// <summary>
    ///128 	Code 128, switching code subset A, B, C automatically 
    ///128M 	Code 128, switching code subset A, B, C manually.
    ///EAN128 Code 128, switching code subset A, B, C automatically 
    ///25 	Interleaved 2 of 5 
    ///25C Interleaved 2 of 5 with check digits
    ///39 	Code 39
    ///39C Code 39 with check digits
    ///93	Code 93 
    ///EAN13 EAN 13
    ///EAN13+2 EAN 13 with 2 digits add-on
    ///EAN13+5 EAN 13 with 5 digits add-on
    ///EAN8    EAN 8 
    ///EAN8+2 	EAN 8 with 2 digits add-on
    ///EAN8+5 	EAN 8 with 5 digits add-on
    ///CODA    Codabar
    ///POST    Postnet
    ///UPCA    UPC-A
    ///UPCA+2	UPC-A with 2 digits add-on
    ///UPCA+5	UPC-A with 5 digits add-on
    ///UPCE    UPC-E
    ///UPCE+2	UPC-E with 2 digits add-on
    ///UPCE+5	UPC-E with 5 digits add-on
    /// </summary>
    public virtual string BarcodeType { get; set; } = "128";
    /// <summary>
    ///Sets up barcode height, given in points
    /// </summary>
    public virtual int BarcodeHeight { get; set; }

    /// <summary>
    /// Align barcode content text under barcode rectangle.
    /// </summary>
    public Alignment TextAlignment { get; set; } = Alignment.Center;
    /// <summary>
    /// Align barcode rectangle in label.
    /// </summary>
    public virtual Alignment Alignment { get; set; } = Alignment.Center;
    /// <summary>
    ///Sets up rotation degrees
    ///     0: rotates 0 degree
    ///     90: rotates 90 degrees
    ///     180: rotates 180 degrees
    ///     270: rotates 270 degrees
    /// </summary>
    public virtual int Rotation { get; set; }

    /// <summary>
    ///Width of narrow element (in mm)
    /// </summary>
    public virtual float NarrowRatio { get; set; } = 1;

    /// <summary>
    ///Width of wide element (in mm)
    /// </summary>
    public virtual float WideRatio { get; set; } = 1;
    /// <summary>
    ///Barcode content
    /// </summary>
    public virtual string Barcode { get; set; }

    public virtual string FullBarcode
    {
      get
      {
        return AppendCrcToBarcode && !string.IsNullOrEmpty(Barcode) ? CRCHelper.CalculateCRC(Barcode) + Barcode : Barcode;
      }
    }
    public virtual bool AppendCrcToBarcode { get; set; }
    public BarcodeInfo Series { get; set; }
  }
}
