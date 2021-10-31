using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum StockCheckingTagStatus : byte
  {
    None = 0,
    NotCounted = 1,
    CorrectCounting = 2,
    Contradiction = 3,
    NonSerialTag = 4,
    NotInventory = 5
  }
}
