using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.PrinterManagment
{
  public class PrinterSetting
  {
    /// <summary>
    /// Barcode paper or sheet width
    /// </summary>
    public virtual int PaperWidth { get; set; }
    public virtual int PaperHeight { get; set; }
    public virtual int PaperMargin { get; set; }
    /// <summary>
    /// Sets up label width; unit: mm
    /// </summary>
    public virtual int LabelWidth { get; set; }
    /// <summary>
    /// Sets up label height; unit: mm
    /// </summary>
    public virtual int LabelHeight { get; set; }
    public virtual int LabelPadding { get; set; }
    public PrintDirection Direction { get; set; } = PrintDirection.LeftUpperCorner;
    /// <summary>
    ///Defines the reference point of the label. The reference (origin) point varies with the print direction
    /// </summary>
    public PointF ReferencePoint { get; set; }

    /// <summary>
    /// Sets up print speed, (selectable print speeds vary on different printer models)
    ///1.0: sets print speed at 1.0"/sec 
    ///1.5: sets print speed at 1.5"/sec 
    ///2.0: sets print speed at 2.0"/sec 
    ///3.0: sets print speed at 3.0"/sec 
    ///4.0: sets print speed at 4.0"/sec 
    ///6.0: sets print speed at 6.0"/sec
    ///8.0: sets print speed at 8.0"/sec
    ///10.0: sets print speed at 10.0"/sec
    ///12.0: sets print speed at 12.0"/sec
    /// </summary>
    public virtual int PrintSpeed { get; set; } = 3;

    /// <summary>
    /// Sets up print density 0~15 ，the greater the number, the darker the printing
    /// </summary>
    public virtual int PrintDensity { get; set; } = 15;
    /// <summary>
    ///Sets up the sensor type to be used
    ///0: signifies that vertical gap sensor is to be used
    ///1: signifies that black mark sensor is to be used
    /// </summary>
    public virtual int SensorType { get; set; }
    /// <summary>
    ///Sets up vertical gap height of the gap/black mark; unit: mm
    /// </summary>
    public virtual int VerticalGap { get; set; }

    public virtual int HorizontalGap { get; set; }
    public virtual int PrintDpi { get; set; } = 200;
    /// <summary>
    ///Sets up shift distance of the gap/black mark; unit:: mm; in the 
    ///case of the average label, set this parameter to be 0.
    /// </summary>
    public virtual int ShiftDistance { get; set; }

  }

  public enum SensorType
  {
    VerticalGap = 0,
    BlackMark = 1
  }
}
