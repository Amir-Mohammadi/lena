using System;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.CalendarEvent
{
  public class CalendarEventResult
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public DateTime DateTime { get; set; }
    public long Duration { get; set; }
    public DateTime ToDateTime => DateTime.AddSeconds(Duration);
    public byte[] RowVersion { get; set; }
  }
}
