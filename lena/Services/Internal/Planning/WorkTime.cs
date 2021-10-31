using System;
// using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Planning.WorkTime;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public CalendarEvent AddWorkTime(
        DateTime dateTime,
        long duration,
        int workShiftId)
    {
      var workTime = repository.Create<CalendarEvent>();
      var result = App.Internals.ApplicationBase
                .AddCalendarEvent(workTime,
                dateTime: dateTime,
                duration: duration,
                workShiftId: workShiftId,
                type: CalendarEventType.WorkTime);
      return result;
    }
    public IQueryable<CalendarEvent> GetWorkTimes(TValue<int> id = null,
        TValue<DateTime> dateTime = null,
        TValue<long> fromTime = null,
        TValue<long> duration = null,
        TValue<int?> workShiftId = null)
    {
      var calendarEventQuery = App.Internals.ApplicationBase
                .GetCalendarEvents(id: id,
                dateTime: dateTime,
                duration: duration,
                workShiftId: workShiftId,
                type: CalendarEventType.WorkTime);
      return calendarEventQuery;
    }
    public CalendarEvent GetWorkTime(int id)
    {
      var calendarEvent = this.GetWorkTimes(id: id).FirstOrDefault();
      if (calendarEvent == null)
        throw new CalendarEventNotFoundException(id);
      return calendarEvent;
    }
    public CalendarEvent EditWorkTime(
        int id,
        byte[] rowVersion,
        TValue<DateTime> dateTime = null,
        TValue<long> fromTime = null,
        TValue<long> duration = null,
        TValue<int?> workShiftId = null
        )
    {
      var workTime = this.GetWorkTime(id: id);
      var result = App.Internals.ApplicationBase.EditCalendarEvent(
                calendarEvent: workTime,
                rowVersion: rowVersion,
                dateTime: dateTime,
                duration: duration,
                workShiftId: workShiftId);
      return result;
    }
    public void DeleteWorkTime(int id)
    {
      var workTime = GetWorkTime(id);
      repository.Delete(workTime);
    }
    public IQueryable<WorkTimeResult> ToWorkTimeResultQuery(IQueryable<CalendarEvent> workTimes)
    {
      var results =
          from workTime in workTimes
          let workShift = workTime.WorkShift
          select new WorkTimeResult
          {
            Id = workTime.Id,
            DateTime = workTime.DateTime,
            WorkShiftId = workTime.WorkShiftId,
            WorkShiftName = workShift.Name,
            Duration = workTime.Duration,
            RowVersion = workTime.RowVersion,
          };
      return results;
    }
    public IQueryable<WorkTimeResult> SearchWorkTimeResultQuery(
        IQueryable<WorkTimeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        DateTime fromDate,
        DateTime toDate,
        int? workShiftId = null
        )
    {
      query = from workTime in query
              where workTime.DateTime >= fromDate && workTime.DateTime <= toDate
              select workTime;
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.WorkShiftName.Contains(searchText)
                select item;
      }
      if (workShiftId.HasValue)
      {
        query = from item in query
                where item.WorkShiftId == workShiftId
                select item;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    public WorkTimeResult ToWorkTimeResult(CalendarEvent workTime)
    {
      var workShift = workTime.WorkShift;
      return new WorkTimeResult
      {
        Id = workTime.Id,
        DateTime = workTime.DateTime,
        Duration = workTime.Duration,
        WorkShiftId = workTime.WorkShiftId,
        WorkShiftName = workShift.Name,
        RowVersion = workTime.RowVersion,
      };
    }
    public IOrderedQueryable<WorkTimeResult> SortWorkTimeResult(IQueryable<WorkTimeResult> input, SortInput<WorkTimeSortType> options)
    {
      switch (options.SortType)
      {
        case WorkTimeSortType.Id:
          return input.OrderBy(r => r.Id, options.SortOrder);
        case WorkTimeSortType.DateTime:
          return input.OrderBy(r => r.DateTime, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(options.SortOrder), options.SortType, null);
      }
    }
    public void AddWeekWorkTime(
        DateTime fromDate,
        DateTime toDate,
        long fromTime,
        long duration,
        int? workShiftId,
        DayOfWeek dayOfWeek)
    {
      fromDate = fromDate.ToLocalTime();
      toDate = toDate.ToLocalTime();
      while (fromDate <= toDate)
      {
        if (fromDate.DayOfWeek == dayOfWeek)
        {
          var workTime = repository.Create<CalendarEvent>();
          App.Internals.ApplicationBase
                    .AddCalendarEvent(workTime,
                        dateTime: fromDate.AddSeconds(fromTime).ToUniversalTime(),
                        duration: duration,
                        workShiftId: workShiftId,
                        type: CalendarEventType.WorkTime);
        }
        fromDate = fromDate.AddDays(1);
      }
    }
    public void AddWeekWorkTimes(
        DateTime fromDate,
        DateTime toDate,
        AddWeekWorkTimeInput[] weekWorkTimeInputs,
        bool onlyApplyToWorkDays
        )
    {
      foreach (var addWeekWorkTimeInput in weekWorkTimeInputs)
        AddWeekWorkTime(
                  fromDate: fromDate,
                  toDate: toDate,
                  fromTime: addWeekWorkTimeInput.FromTime,
                  duration: addWeekWorkTimeInput.Duration,
                  workShiftId: addWeekWorkTimeInput.WorkShiftId,
                  dayOfWeek: addWeekWorkTimeInput.DayOfWeek);
    }
    public void AddDayWorkTime(
        DateTime fromDate,
        DateTime toDate,
        long fromTime,
        long duration,
        int workShiftId,
        int dayNumber,
        bool onlyApplyToWorkDays,
        int period
        )
    {
      fromDate = fromDate.ToLocalTime();
      toDate = toDate.ToLocalTime();
      fromDate = fromDate.AddDays(dayNumber - 1);
      while (fromDate <= toDate)
      {
        var workTime = repository.Create<CalendarEvent>();
        App.Internals.ApplicationBase
                  .AddCalendarEvent(workTime,
                      dateTime: fromDate.AddSeconds(fromTime).ToUniversalTime(),
                      duration: duration,
                      workShiftId: workShiftId,
                      type: CalendarEventType.WorkTime);
        fromDate = fromDate.AddDays(period);
      }
    }
    public void AddDayWorkTimes(
        DateTime fromDate,
        DateTime toDate,
        AddDayWorkTimeInput[] dayWorkTimeInputs,
        bool onlyApplyToWorkDays,
        int period
        )
    {
      var maxDay = dayWorkTimeInputs.Max(i => i.DayNumber);
      if (period < maxDay)
        throw new InvalidPeriodWorkTimeException(maxDay, period);
      foreach (var addWeekWorkTimeInput in dayWorkTimeInputs)
        AddDayWorkTime(
                      fromDate: fromDate,
                      toDate: toDate,
                      fromTime: addWeekWorkTimeInput.FromTime,
                      duration: addWeekWorkTimeInput.Duration,
                      workShiftId: addWeekWorkTimeInput.WorkShiftId,
                      dayNumber: addWeekWorkTimeInput.DayNumber,
                      onlyApplyToWorkDays: onlyApplyToWorkDays,
                      period: period);
    }
  }
}