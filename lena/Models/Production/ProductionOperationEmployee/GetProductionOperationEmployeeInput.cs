using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperationEmployee
{
  public class GetProductionOperationEmployeeInput
  {
    public int? Id { get; set; }
    public int ProductionId { get; set; }
    public int? EmployeeId { get; set; }
    public int? TerminalId { get; set; }
    public int? ProductionOperationId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public int? ProductionLineId { get; set; }
    public int?[] ProductionLineIds { get; set; }
    public int[] ProductionOperationIds { get; set; }
    public string ProductionOrderCode { get; set; }
    public int? ProductionTerminalId { get; set; }
    public int? OperationId { get; set; }
    public int? StuffId { get; set; }
    public string Serial { get; set; }
  }
}
