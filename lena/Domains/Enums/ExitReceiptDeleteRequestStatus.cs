using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum ExitReceiptDeleteRequestStatus : byte
  {
    NotAction, // اقدام نشده
    Accepted, // تایید شده
    Rejected // رد شده
  }
}
