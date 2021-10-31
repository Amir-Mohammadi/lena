using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionLineEmployeeInterval
{
  public class ProductionLineOperationEmployeeTimeResult
  {
    public DateTime Date { get; set; }
    public int? OperationId { get; set; }
    public int StuffId { get; set; }
    public int? ProductionLineId { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public double? Duration { get; set; } = null;

  }
}
