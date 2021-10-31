using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum EssentialContactType : byte
  {
    Employee = 1,
    Customer = 2,
    Provider = 4,
  }
}
