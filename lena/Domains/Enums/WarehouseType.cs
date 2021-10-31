using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum WarehouseType : byte
  {
    None = 0,
    Inbound = 1,
    Outbound = 2
  }
}
