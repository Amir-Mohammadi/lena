using lena.Models.Planning.BillOfMaterialDetail;
using lena.Models.WarehouseManagement.Common;
using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class FullBillOfMaterialSummaryResult
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int Version { get; set; }
    public string Title { get; set; }
    public int ProductionStepId { get; set; }
    public string ProductionStepName { get; set; }
    public BillOfMaterialVersionType BillOfMaterialVersionType { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public System.DateTime CreateDate { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double UnitConversionRatio { get; set; }
    public string Description { get; set; }
    public UnitComboResult[] Units { get; set; }
    public FullBillOfMaterialDetailResult[] BillOfMaterialDetails { get; set; }
    public bool? HasProduction { get; set; }
    public bool? HasProductionPlan { get; set; }
    public bool? HasProductionSchedule { get; set; }
    public byte[] RowVersion { get; set; }
  }
}