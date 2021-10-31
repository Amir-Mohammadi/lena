using System.Linq;
using lena.Models.Planning.ProductionLineWorkShift;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionSchedule
{
  public class ProductionLineProductionSchedulesResult
  {
    public int SortIndex { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public IQueryable<ProductionLineWorkTimeResult> ProductionLineWorkTimes { get; set; }
    public IQueryable<ProductionScheduleResult> ProductionSchedules { get; set; }
  }
}
