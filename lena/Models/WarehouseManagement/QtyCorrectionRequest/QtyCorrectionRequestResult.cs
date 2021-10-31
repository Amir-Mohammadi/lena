using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.QtyCorrectionRequest
{
  public class QtyCorrectionRequestResult
  {
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public long? SerialCode { get; set; }
    public string Serial { get; set; }
    public QtyCorrectionRequestType Type { get; set; }
    public QtyCorrectionRequestStatus Status { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
