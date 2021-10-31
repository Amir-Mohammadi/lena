using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperationSequence
{
  public class GetProductionOperationSequeneceInput
  {
    public int WorkPlanStepId { get; set; }
    public int? TerminalId { get; set; }
    /// <summary>
    /// شماره دستور کار
    /// </summary>
    public string ProductionOrderCode { get; set; }
  }
}
