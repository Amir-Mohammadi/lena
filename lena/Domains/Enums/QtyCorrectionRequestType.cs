using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum QtyCorrectionRequestType : byte
  {
    Missing, // مفقودی
    IncreaseAmount, // افزایش موجودی
    DecreaseAmount, // کاهش موجودی
    IncreaseStockChecking, // افزایش موجودی انبارگردانی
    DecreaseStockChecking // کاهش موجودی انبارگردانی
  }
}
