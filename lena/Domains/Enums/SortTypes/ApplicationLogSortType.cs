using lena.Domains.Enums;
namespace lena.Domains.Enums.SortTypes
{
  public enum ApplicationLogSortType
  {
    Id,
    ClientIp,
    UserAgent,
    Action,
    UserName,
    EmployeeCode,
    EmployeeFullName,
    LogTime,
    RequestEndTime,
    //For Grouped Min Max Result
    Date,
    FirstRequestTime,
    LastRequestTime,
    TotalTime
  }
}
