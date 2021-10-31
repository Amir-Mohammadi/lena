using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.NewShoppingSerial
{
  public class NewShoppingSerialResult
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string Serial { get; set; }
    public long StuffSerialCode { get; set; }
    public double? InitQty { get; set; }
    public int UnitId { get; set; }
    public string UnitName { get; set; }
    public double? PartitionStuffSerialQty { get; set; }
    public string MainSerial { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
