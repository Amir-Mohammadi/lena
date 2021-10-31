using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CalendarEvent : IEntity
  {
    protected internal CalendarEvent()
    {
    }
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public long Duration { get; set; }
    public CalendarEventType Type { get; set; }
    public Nullable<int> WorkShiftId { get; set; }
    public virtual WorkShift WorkShift { get; set; }
    public virtual ProductionSchedule ProductionSchedule { get; set; }
    public virtual ProductionOrder ProductionOrder { get; set; }
    public byte[] RowVersion { get; set; }
  }
}