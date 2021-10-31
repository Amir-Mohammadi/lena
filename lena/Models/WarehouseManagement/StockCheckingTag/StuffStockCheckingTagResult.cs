using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockCheckingTag
{
  public class StuffStockCheckingTagResult
  {
    public int? Id { get; set; }
    public int? Number { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public StuffType StuffType { get; set; }
    public int StuffCategoryId { get; set; }
    public string StuffCategoryName { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public int TagTypeId { get; set; }
    public string TagTypeName { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int StockCheckingId { get; set; }
    public string StockCheckingTitle { get; set; }
    public double? Amount { get; set; }
    public int? UnitId { get; set; }
    public string UnitName { get; set; }
    public double? UnitConversionRatio { get; set; }
    public string TagSerial { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
