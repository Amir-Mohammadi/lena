using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStuffDetail
{
  public class ProductionStuffDetailResult
  {
    public int Id { get; set; }
    public int? ProductionId { get; set; }
    public int? ProductionOperationId { get; set; }
    public string ProductionOperationName { get; set; }
    public BillOfMaterialDetailType BillOfMaterialDetailType { get; set; }
    public int StuffId { get; set; }
    public long? StuffSerialCode { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public byte[] RowVersion { get; set; }
    public short WarehouseId { get; set; }
    public ProductionStuffDetailType Type { get; set; }
    public double DetachedQty { get; set; }
    public string StuffName { get; set; }
    public string UnitName { get; set; }
    public string WarehouseName { get; set; }
    public string Serial { get; set; }
    public string StuffCode { get; set; }
    public int? EquivalentStuffId { get; set; }
  }
}
