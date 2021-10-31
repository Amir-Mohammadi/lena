using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.ApplicationBase.Calendar;
using lena.Models.Common;
using Calendar = lena.Domains.Calendar;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    public IQueryable<Calendar> GetCalendars(
            TValue<DateTime> date = null,
            TValue<bool> isWorkingDay = null,
            TValue<bool> isHoliday = null)
    {

      var isDateNull = date == null;
      var isIsWorkingDayNull = isWorkingDay == null;
      var isIsHolidayNull = isHoliday == null;
      var calendars = from calendar in repository.GetQuery<Calendar>()
                      where
                                (isDateNull || calendar.Date == date) &&
                                (isIsWorkingDayNull || calendar.IsWorkingDay == isWorkingDay) &&
                                (isIsHolidayNull || calendar.IsHoliday == isHoliday)
                      select calendar;
      return calendars;
    }
    public Calendar GetCalendar(DateTime date)
    {

      var calendar = this.GetCalendars(date: date).FirstOrDefault();
      if (calendar == null)
        throw new CalendarNotFoundException(date);
      return calendar;
    }
    public Calendar AddCalendar(
            DateTime date,
            bool isWorkingDay,
            bool isHoliday)
    {

      var calendar = repository.Create<Calendar>();
      calendar.Date = date;
      calendar.IsWorkingDay = isWorkingDay;
      calendar.IsHoliday = isHoliday;
      repository.Add(calendar);
      return calendar;
    }
    public Calendar EditCalendar(
            byte[] rowVersion,
            DateTime date,
            TValue<bool> isWorkingDay = null,
            TValue<bool> isHoliday = null)
    {

      var calendar = GetCalendar(date: date);
      if (isWorkingDay != null)
        calendar.IsWorkingDay = isWorkingDay;
      if (isHoliday != null)
        calendar.IsHoliday = isHoliday;
      repository.Update(entity: calendar, rowVersion: rowVersion);
      return calendar;
    }
    public void DeleteCalendar(DateTime date)
    {

      var calendar = GetCalendar(date: date);
      repository.Delete(calendar);
    }
    public IQueryable<CalendarResult> ToCalendarResultQuery(IQueryable<Calendar> query)
    {
      var calendars = from calendar in query
                      select new CalendarResult
                      {
                        Date = calendar.Date,
                        IsWorkingDay = calendar.IsWorkingDay,
                        IsHoliday = calendar.IsHoliday,
                        RowVersion = calendar.RowVersion
                      };
      return calendars;
    }
    public CalendarResult ToCalendarResult(Calendar calendar)
    {
      return new CalendarResult
      {
        Date = calendar.Date,
        IsWorkingDay = calendar.IsWorkingDay,
        IsHoliday = calendar.IsHoliday,
        RowVersion = calendar.RowVersion
      };
    }
    public IOrderedQueryable<CalendarResult> SortCalendarResult(IQueryable<CalendarResult> query, SortInput<CalendarSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case CalendarSortType.Date:
          return query.OrderBy(r => r.Date, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(sortInput.SortOrder), sortInput.SortType, null);
      }
    }
    public IQueryable<CalendarResult> SearchCalendarResult(IQueryable<CalendarResult> query, DateTime? fromDate, DateTime? toDate)
    {
      var isFromDateNull = fromDate == null;
      var isToDateNull = toDate == null;
      var calendars = from calendar in query
                      where
                      (isFromDateNull || calendar.Date >= fromDate) &&
                      (isToDateNull || calendar.Date <= toDate)
                      select calendar;
      return calendars;

    }
    public void AddYear(DateTime fromDate, DateTime toDate)
    {

      fromDate = fromDate.ToLocalTime().Date;
      toDate = toDate.ToLocalTime();
      while (fromDate < toDate)
      {
        var calendar = repository.Create<Calendar>();
        calendar.Date = fromDate.ToUniversalTime();
        var isFriday = fromDate.DayOfWeek == DayOfWeek.Friday;
        AddCalendar(date: calendar.Date, isWorkingDay: !isFriday, isHoliday: isFriday);
        fromDate = fromDate.AddDays(1);
      }
    }
  }
}
