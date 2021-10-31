using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum DetailCodeConfirmationStatus : byte
  {
    NotAction = 0,
    Accepted = 1,
    Rejected = 2,
  }
}
