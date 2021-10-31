using System;
using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum ProjectERPEventAnnouncementType : short
  {
    None = 0,
    ByPhone = 1,
    ByEmail = 2,
    ByFax = 4,
    Others = 8,
  }
}