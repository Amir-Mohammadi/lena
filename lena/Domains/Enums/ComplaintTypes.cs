using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum ComplaintTypes : byte
  {
    None = 0,
    ByPhone = 1,
    ByEmail = 2,
    ByFax = 4,
    Others = 8,
  }
}