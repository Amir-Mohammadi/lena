using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStuffDetail
{
  public class GetProductionStuffDetailInput
  {
    public int? Id { get; set; }
    public int? ProductionId { get; set; }
    public int? StuffId { get; set; }
    public int? WarehouseId { get; set; }
    public string Serial { get; set; }
    public bool? DoNotShowCompleteDetachedItems { get; set; }
    public string ProductionSerial { get; set; }
  }
}
