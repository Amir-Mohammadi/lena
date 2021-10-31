using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperator
{
  public class ProductionOperatorTempResult
  {
    public int OperationId { get; set; }
    public string OperationCode { get; set; }
    public string ProductionOrderCode { get; set; }
    public int ProductionOrderId { get; set; }
    public int? OperationSequenceId { get; set; }
  }
}
