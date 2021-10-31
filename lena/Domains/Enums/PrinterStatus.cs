using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum PrinterStatus : byte
  {
    Other = 1,
    Unknown,
    Idle,
    Printing,
    Warmup,
    WarmingUp,
    StoppedPrinting,
    Offline
  }
}
