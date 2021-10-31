using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  public class StuffSerialInfoResult
  {
    public long Code { get; set; }
    public int StuffId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public StuffType StuffType { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public string StuffTitle { get; set; }
    public int SerialProfileCode { get; set; }
    public long BatchNo { get; set; }
    public int Order { get; set; }
    public string Serial { get; set; }
    public double InitQty { get; set; }
    public int InitUnitId { get; set; }
    public System.DateTime? DateTime { get; set; }
    public string InitUnitName { get; set; }
    public double InitUnitConversionRatio { get; set; }
    public StuffSerialStatus Status { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public int? WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public byte[] RowVersion { get; set; }


    public long? StuffSerialCode { get; set; }
    public string StuffCategoryName { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public double? TotalAmount { get; set; }
    public double? AvailableAmount { get; set; }
    public double? BlockedAmount { get; set; }
    public double? QualityControlAmount { get; set; }
    public double? WasteAmount { get; set; }
    public int? UnitId { get; set; }
    public string UnitName { get; set; }
    public string StockPlaceCodes { get; set; }
    public string StockPlaceTitles { get; set; }

    public int? ProductionId { get; set; }
    public string ProductionCode { get; set; }

  }
}
