using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ReturnOfSaleSerial
{
  public class ReturnOfSaleSerialResult
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public double? Qty { get; set; }
    public ReturnOfSaleStatus Status { get; set; }
    public byte UnitId { get; set; }
    public double? PartitionStuffSerialQty { get; set; }
    public int? MainStuffId { get; set; }
    public string MainStuffName { get; set; }
    public string MainStuffCode { get; set; }
    public string ExitReceiptCode { get; set; }
    public string UnitName { get; set; }
    public ReturnOfSaleType Type { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
