using System.Linq;

using lena.Domains.Enums;
namespace lena.Models
{
  public class IndicatorWeightResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public short? DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public IQueryable<WeightDayResult> WeightDayResults { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
