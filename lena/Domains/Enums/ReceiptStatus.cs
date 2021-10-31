using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum ReceiptStatus : byte
  {
    None = 0,
    Temporary = 1,
    NoReceipt = 2,
    TemporaryReceipt = 4,
    EternalReceipt = 8,
    Priced = 16,
  }
}
