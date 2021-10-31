using Newtonsoft.Json;
using lena.Services.Core.Foundation;
using lena.Domains;
using lena.Models.ApplicationBase.CalendarEvent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class InterferenceScheduleCalendarEventException : InternalServiceException
  {
    public CalendarEventResult[] CalendarEvent { get; set; }

    public InterferenceScheduleCalendarEventException(IList<CalendarEventResult> calendarEvent)
    {
      this.CalendarEvent = calendarEvent.ToArray();
    }

  }


}
//CustomException e = new CustomException(msg);
//e.setStudents(list);
//throw e;