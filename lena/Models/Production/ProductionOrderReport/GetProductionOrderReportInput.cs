using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrderReport
{
  public class GetProductionOrderReportInput
  {
    public int? Id { get; set; }
    public string Code { get; set; }
    public int WorkPlanStepId { get; set; }
    public int ProductionTerminalId { get; set; }

  }
}
