using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStuffDetail
{
  public class AddRepairProductionStuffDetailInput
  {
    public ProductionStuffDetailType ProductionStuffDetailType { get; set; }
    public int StuffId { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public long? StuffSerialCode { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }

    public int? ParentOperationId { get; set; }

    // [Backend only]
    public int? RepairProductoinFaultId { get; set; }
  }



}
