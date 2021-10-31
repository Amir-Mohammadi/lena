using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.ApplicationBase.CalendarEvent;
using lena.Models.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    public IQueryable<CalendarEvent> GetCalendarEvents(
            TValue<int> id = null,
            TValue<DateTime> dateTime = null,
            TValue<long> duration = null,
            TValue<int?> workShiftId = null,
            TValue<CalendarEventType?> type = null,
            TValue<int?> productionScheduleId = null)
    {


      var query = repository.GetQuery<CalendarEvent>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (dateTime != null)
        query = query.Where(i => i.DateTime == dateTime);
      if (duration != null)
        query = query.Where(i => i.Duration == duration);
      if (workShiftId != null)
        query = query.Where(i => i.WorkShiftId == workShiftId);
      if (type != null)
        query = query.Where(i => i.Type == type);
      if (productionScheduleId != null)
        query = query.Where(i => i.ProductionSchedule.Id == productionScheduleId);
      return query;
    }
    public CalendarEvent GetCalendarEvent(int id)
    {

      var calendarEvent = this.GetCalendarEvents(id: id).FirstOrDefault();
      if (calendarEvent == null)
        throw new CalendarEventNotFoundException(id);
      return calendarEvent;
    }
    public CalendarEvent AddCalendarEvent(CalendarEvent calendarEvent,
            DateTime dateTime,
            long duration,
            int? workShiftId,
            CalendarEventType type)
    {

      calendarEvent.DateTime = dateTime;
      calendarEvent.Duration = duration;
      calendarEvent.WorkShiftId = workShiftId;
      calendarEvent.Type = type;
      repository.Add(calendarEvent);
      return calendarEvent;
    }
    public CalendarEvent EditCalendarEvent(
        int id,
        byte[] rowVersion,
        TValue<DateTime> dateTime = null,
        TValue<long> duration = null,
        TValue<int?> workShiftId = null)
    {

      var calendarEvent = GetCalendarEvent(id);

      EditCalendarEvent(
                    calendarEvent: calendarEvent,
                    rowVersion: rowVersion,
                    dateTime: dateTime,
                    duration: duration,
                    workShiftId: workShiftId);
      return calendarEvent;
    }

    public CalendarEvent EditCalendarEvent(
        CalendarEvent calendarEvent,
        byte[] rowVersion,
        TValue<DateTime> dateTime = null,
        TValue<long> duration = null,
        TValue<int?> workShiftId = null)
    {

      if (dateTime != null)
        calendarEvent.DateTime = dateTime;
      if (duration != null)
        calendarEvent.Duration = duration;
      if (workShiftId != null)
        calendarEvent.WorkShiftId = workShiftId;

      repository.Update(entity: calendarEvent, rowVersion: rowVersion);
      return calendarEvent;
    }

    public void DeleteCalendarEvent(int id)
    {

      var calendarEvent = GetCalendarEvent(id);
      repository.Delete(calendarEvent);
    }
    public IQueryable<CalendarEventResult> ToCalendarEventResultQuery(IQueryable<CalendarEvent> query)
    {
      var calendarEvents = from calendarEvent in query

                           select new CalendarEventResult
                           {
                             Id = calendarEvent.Id,
                             DateTime = calendarEvent.DateTime,
                             Duration = calendarEvent.Duration,
                             RowVersion = calendarEvent.RowVersion,
                           };
      return calendarEvents;
    }
    public IQueryable<CalendarEventResult> ToCalendarEventResultQueryWithStuffId(IQueryable<CalendarEvent> query)
    {
      var calendarEvents = from calendarEvent in query

                           select new CalendarEventResult
                           {
                             Id = calendarEvent.Id,
                             DateTime = calendarEvent.DateTime,
                             Duration = calendarEvent.Duration,
                             StuffId = calendarEvent.ProductionSchedule.WorkPlanStep.WorkPlan.BillOfMaterialStuffId,
                             RowVersion = calendarEvent.RowVersion,
                           };


      return calendarEvents;
    }
    public CalendarEventResult ToCalendarEventResult(CalendarEvent calendarEvent)
    {

      return new CalendarEventResult
      {
        Id = calendarEvent.Id,
        DateTime = calendarEvent.DateTime,
        Duration = calendarEvent.Duration,
        RowVersion = calendarEvent.RowVersion,
      };
    }
    public IOrderedQueryable<CalendarEventResult> SortCalendarEventResult(IQueryable<CalendarEventResult> query, SortInput<CalendarEventSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case CalendarEventSortType.DateTime:
          return query.OrderBy(r => r.DateTime, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(sortInput.SortOrder), sortInput.SortType, null);
      }
    }
  }
}
