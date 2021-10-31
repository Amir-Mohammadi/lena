using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialTracking
{
  public class SerialTrackingConsumptionResult
  {
    public string LinkedSerial { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string Serial { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double InitQty { get; set; }
    public double PartitionedQty { get; set; }
    public int InitUnitId { get; set; }
    public string InitUnitName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
