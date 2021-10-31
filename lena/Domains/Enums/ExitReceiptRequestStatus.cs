using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{

  [Flags]
  public enum ExitReceiptRequestStatus : short
  {
    None = 0,
    Waiting = 1,
    GettingPermission = 2,
    Permissioned = 4,
    Preparing = 8,
    Prepared = 16,
    Sending = 32,
    Sended = 64
  }
}
