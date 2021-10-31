using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.PrinterManagment
{
  public enum PrintDirection
  {
    /// <summary>
    /// Print from left upper corner of barcode paper( Up to Down).
    /// </summary>
    LeftUpperCorner,
    /// <summary>
    /// Print from right down corner of barcode paper (Down to Up).
    /// </summary>
    RightDownCorner
  }
}
