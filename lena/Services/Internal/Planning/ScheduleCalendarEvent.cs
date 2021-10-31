using System;
//using System.Data.Entity.SqlServer;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using Microsoft.EntityFrameworkCore;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public CalendarEvent AddScheduleCalendarEvent(
        DateTime dateTime,
        long duration,
        ProductionSchedule productionSchedule)
    {

      var scheduleCalendarEvent = repository.Create<CalendarEvent>();

      var result = App.Internals.ApplicationBase
                .AddCalendarEvent(calendarEvent: scheduleCalendarEvent,
                    dateTime: dateTime,
                    duration: duration,
                    workShiftId: null,
                    type: CalendarEventType.Schedule);
      return scheduleCalendarEvent;
    }

    public IQueryable<CalendarEvent> GetScheduleCalendarEvents(TValue<int> id = null,
        TValue<DateTime> dateTime = null,
        TValue<long> duration = null,
        TValue<int?> productionScheduleId = null)
    {

      var calendarEventQuery = App.Internals.ApplicationBase
                .GetCalendarEvents(id: id,
                    dateTime: dateTime,
                    duration: duration,
                    type: CalendarEventType.Schedule,
                    productionScheduleId: productionScheduleId);
      return calendarEventQuery;
    }

    public CalendarEvent GetScheduleCalendarEvent(int id)
    {

      var calendarEvent = this.GetScheduleCalendarEvents(id: id)


                .FirstOrDefault();
      if (calendarEvent == null)
        throw new CalendarEventNotFoundException(id);
      return calendarEvent;
    }

    public IQueryable<CalendarEvent> CheckInterferenceScheduleCalendarEvent(
        DateTime psFromDateTime,
        long duration,
        int productionLineId)
    {

      var scheduleCalendarEvents = this.GetScheduleCalendarEvents();
      var psToDateTime = psFromDateTime.AddSeconds(duration);
      scheduleCalendarEvents = from item in scheduleCalendarEvents
                               let fromDateTime = item.DateTime
                               let toDateTime = fromDateTime.AddSeconds(item.Duration)
                               where item.ProductionSchedule.IsDelete == false &&
                                     (item.ProductionSchedule.WorkPlanStep.ProductionLineId == productionLineId) &&
                                           ((fromDateTime <= psFromDateTime && toDateTime > psFromDateTime) ||
                                           (fromDateTime < psToDateTime && toDateTime >= psToDateTime) ||
                                           (fromDateTime <= psFromDateTime && toDateTime >= psToDateTime) ||
                                           (fromDateTime >= psFromDateTime && toDateTime <= psToDateTime))
                               select item;
      return scheduleCalendarEvents;
    }
  }
}
