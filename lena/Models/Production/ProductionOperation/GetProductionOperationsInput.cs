using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperation
{
  public class GetProductionOperationsInput
  {

    public int? Id { get; set; }
    public int? UserId { get; set; }
    public int? ProductionId { get; set; }
    public int? ProductionOperatorId { get; set; }
    public string ProductionOrderCode { get; set; }
    public int? ProductionOrderId { get; set; }
    public int? ProductionTerminalId { get; set; }
    public string Serial { get; set; }
    public bool? IsCorrectionOperation { get; set; }
    public ProductionOperationStatus? Status { get; set; }
    public ProductionOperationStatus[] Statuses { get; set; }
    public ProductionOperationStatus[] NotHasStatuses { get; set; }
  }
}
