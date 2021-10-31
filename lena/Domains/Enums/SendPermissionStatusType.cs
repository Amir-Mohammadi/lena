using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum SendPermissionStatusType : short
  {
    None = 0,
    Waiting = 1,
    Accepted = 2,
    Rejected = 4,
    Preparing = 8,
    Prepared = 16,
    Sending = 32,
    Sended = 64
  }
}
