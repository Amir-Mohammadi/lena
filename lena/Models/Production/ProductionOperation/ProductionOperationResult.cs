using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperation
{
  public class ProductionOperationResult
  {
    public int Id { get; set; }
    public long Time { get; set; }
    public int ProductionId { get; set; }
    public int? ProductionOperatorId { get; set; }
    public int OperationId { get; set; }
    public string OperationCode { get; set; }
    public string OperationName { get; set; }
    public int? ProductionTerminalId { get; set; }
    public bool IsFaultCause { get; set; }
    public string ProductionTerminalName { get; set; }
    public int? OperationSequenceIndex { get; set; }
    public int? OperationSequenceId { get; set; }
    public ProductionOperationStatus Status { get; set; }
  }
}
