using lena.Domains;

using lena.Domains;
namespace lena.Models.StaticData
{
  public static class UserGroups
  {
    static UserGroups()
    {
      StoreReceiptDeleteRequestConfirmers = new UserGroup()
      {
        Id = 291,
        Name = "تایید کنندگان درخواست حذف رسید انبار",
        Description = "کاربران این گروه، امکان تأیید یا رد درخواست حذف رسید انبار را خواهند داشت."
      };

      ExitReceiptDeleteRequestConfirmers = new UserGroup()
      {
        Id = 292,
        Name = "تایید کنندگان درخواست حذف برگه خروج انبار",
        Description = "کاربران این گروه، امکان تأیید یا رد درخواست حذف برگه خروج از انبار را خواهند داشت."
      };

    }

    public static UserGroup StoreReceiptDeleteRequestConfirmers { get; }
    public static UserGroup ExitReceiptDeleteRequestConfirmers { get; }


  }
}
