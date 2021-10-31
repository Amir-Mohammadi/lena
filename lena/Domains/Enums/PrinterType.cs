using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{

  public enum PrinterType : byte
  {
    Laser = 1,
    Joharafshan = 2,
    BarcodePrinter = 4
  }
  [Flags]
  public enum PrinterTypeInput : byte
  {
    Laser = 1,
    Joharafshan = 2,
    BarcodePrinter = 4
  }
}
