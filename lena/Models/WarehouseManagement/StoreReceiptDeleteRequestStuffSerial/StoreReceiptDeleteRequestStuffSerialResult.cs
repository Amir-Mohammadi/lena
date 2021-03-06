using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceiptDeleteRequestStuffSerialResult
{
  public class StoreReceiptDeleteRequestStuffSerialResult
  {
    public int Id { get; set; }
    public int StoreReceiptDeleteRequestId { get; set; }
    public int StuffSerialId { get; set; }
    public string Serial { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public long? StuffSerialCode { get; set; }
    public double Amount { get; set; }
    public string UnitName { get; set; }
    public int UnitId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
