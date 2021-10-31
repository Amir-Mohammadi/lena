using System;
using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionLineEmployeeInterval
{
  public class ProductionLineEmployeeIntervalResult
  {
    public int Id { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionLineTitle { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime EntranceDateTime { get; set; }
    public int OperationId { get; set; }
    public IEnumerable<string> OperationTitle { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public DateTime? LastOpretationDateTime { get; set; }
    public DateTime? ExitDateTime { get; set; }
    public int? IntervalDuration { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
