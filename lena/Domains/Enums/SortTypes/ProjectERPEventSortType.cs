using lena.Domains.Enums;
namespace lena.Domains.Enums.SortTypes
{
  public enum ProjectERPEventSortType
  {
    Id,
    ProjectERPTitle,
    ProjectERPEventCategoryName,
    RegisterEmployeeFullName,
    RegisterDateTime, // تاریخ ثبت
    AudienceEmployeeFullName,
    Audience, // مخاطب خارج از سازمان
    Confidential, // محرمانه
    NextAction, // اقدام بعدی
    NextActionDateTime, // تاریخ اقدام بعدی
    Description,
    CRMDateTime,
    Type,
    AnnouncementType, // نحوه اعلام
  }
}
