using System.Linq;
using lena.Models.Planning.ProductionLineWorkShift;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLineWorkShiftWorkTime
{
  public class ProductionLineWorkShiftWorkTimesResult
  {
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public IQueryable<ProductionLineWorkTimeResult> ProductionLineWorkTimes { get; set; }
  }
}
