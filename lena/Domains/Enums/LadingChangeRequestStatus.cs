using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum LadingChangeRequestStatus : byte
  {
    NotAction = 0,
    Accepted = 1,
    Rejected = 2,
  }
}
