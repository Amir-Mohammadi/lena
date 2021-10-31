using System;

using lena.Domains.Enums;
namespace lena.Models.QualityGuarantee.ProductionCapacity
{
  public class ProductionCapacityResult
  {
    public DateTime Date { get; set; }
    public int EmployeeCount { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public int? ProducedCount { get; set; }
    public double Capacity { get; set; }
  }
}
