using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum ProviderType : byte
  {
    Foreign,
    Internal
  }
}
