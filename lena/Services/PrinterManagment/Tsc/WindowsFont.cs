using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.PrinterManagment
{
  public class WindowsFont
  {
    /// <summary>
    ///The starting point of the text along the X , Y direction, given in points
    /// </summary>
    public Point Location { get; set; }
    /// <summary>
    /// The font height, given in points.
    /// </summary>
    public int FontHeight { get; set; }
    /// <summary>
    /// Rotation in counter clockwise direction 
    ///	0 -> 0 degree 
    ///	90-> 90 degree 
    ///	180-> 180 degree 
    ///	270-> 270 degree
    /// </summary>
    public int Rotation { get; set; }
    /// <summary>
    ///font style 
    ///	0-> Normal 
    ///	1-> Italic 
    ///	2-> Bold 
    ///	3-> Bold and Italic
    /// </summary>
    public int FontStyle { get; set; }
    /// <summary>
    /// font with underline 
    ///	0-> Without underline 
    ///	1-> With underline
    /// </summary>
    public int FontWithUnderline { get; set; }
    /// <summary>
    ///Font type face.Specify the true type font name.For example: Arial, Times new Roman.
    /// </summary>
    public string FontFaceType { get; set; }
    /// <summary>
    ///  text to be printed.
    /// </summary>
    public string Text { get; set; }
  }
}
