using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionSchedule
{
  public class ChangeSchedulesPublishStatusInput
  {
    public bool IsPublished { get; set; }
    public List<ScheduleItem> Items { get; set; }
  }
}
