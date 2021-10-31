using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.PrinterManagment
{
  public class BarcodeInfo
  {
    /// <summary>
    ///The starting point of text(character string) along the X , Y direction, given in points(of 200 DPI, 1 point= 1 / 8 mm; of 300 DPI, 1 point=1/12 mm)
    /// </summary>
    public PointF Location { get; set; }
    public float Size { get; set; }
    /// <summary>
    ///Built-in font type name, 12 kinds in sum
    ///1: 8*/12 dots 
    ///2: 12*20 dots 
    ///3: 16*24 dots 
    ///4: 24*32 dots 
    ///5: 32*48 dots 
    ///TST24.BF2: Traditional Chinese 24*24                               
    ///TST16.BF2: Traditional Chinese 16*16                               
    ///TTT24.BF2: Traditional Chinese 24*24 (Telecommunication Code)      
    ///TSS24.BF2: Simplified Chinese 24*24                                
    ///TSS16.BF2: Simplified Chinese 16*16                                 
    ///K: Japan, Korean font 24*24,
    ///L: Japan Korean font 16*16
    /// </summary>
    public string FontTypeName { get; set; } = "1";
    /// <summary>
    ///Sets up the rotation degree of the text(character string)
    ///0: rotates 0 degree
    ///90: rotate 90 degrees
    ///180: rotate 180 degrees
    ///270: rotate 270 degrees
    /// </summary>
    public int Rotation { get; set; } = 0;
    /// <summary>
    ///Sets up the magnification rate of text(character string) along the X direction, range: 1~8
    /// Default value is 1.
    /// </summary>
    public int MagnificationRateX { get; set; } = 1;
    /// <summary>
    ///Sets up the magnification rate of text(character string) along the Y direction, range: 1~8
    /// Default value is 1.
    /// </summary>
    public int MagnificationRateY { get; set; } = 1;
    /// <summary>
    ///Prints the content of text(character string)
    /// </summary>
    public string Text { get; set; } = "";
  }
}
