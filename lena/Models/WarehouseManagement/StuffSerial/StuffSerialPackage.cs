using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  public class StuffSerialPackage
  {
    public long Code { get; set; }
    public int StuffId { get; set; }
    public int BillOfMaterialVersion { get; set; }
    public StuffType StuffType { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public string StuffTitle { get; set; }
    public int SerialProfileCode { get; set; }
    public long BatchNo { get; set; }
    public int Order { get; set; }
    public string Serial { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public System.DateTime? DateTime { get; set; }
    public string InitUnitName { get; set; }
    public double InitUnitConversionRatio { get; set; }
    public StuffSerialStatus Status { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
