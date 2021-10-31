using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Common.Helpers
{
  public class DateTimeHelper
  {
    public static string ConvertUtcToPersianDateTime(DateTime date, string timeFormat = null)
    {
      System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
      var tz = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
      var localTime = TimeZoneInfo.ConvertTimeFromUtc(date, tz);
      var year = pc.GetYear(localTime);
      var month = pc.GetMonth(localTime);
      var day = pc.GetDayOfMonth(localTime);
      return $"{year}/{month.ToString("D2")}/{day.ToString("D2")} {localTime.ToString(localTime != null ? timeFormat : "HH:mm:ss")}";
    }

    public static int GetPersianDateTimeYear(DateTime date)
    {
      PersianCalendar pc = new PersianCalendar();
      return pc.GetYear(date);
    }
  }
}
