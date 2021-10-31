using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum DetailedCodeRequestType : byte
  {
    DetailedCodeConfirmationRequest, // درخواست تایید کد تفصیلی
    PermissionToEditRequest, // در خواست ویرایش کد تفصیلی تایید شده
  }
}
