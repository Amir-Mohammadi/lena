using lena.Domains.Enums;
namespace lena.Domains.Enums.SortTypes
{
  public enum AllocationSortType
  {
    Id,
    BankOrderNumber,
    Amount,
    CurrencyTitle,
    Duration,
    Status, //مرحله اقدام
    ReceivedDateTime, // تاریخ دریافت
    BeginningDateTime, // تاریخ اقدام
    FinalizationDateTime, // تاریخ اتمام
    StatisticalRegistrationCertificate,//شماره گواهی ثبت آماری                  
    EmployeeFullName,
    Description,
    DateTime
  }
}
