using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.EquivalentStuffUsage
{
  public class EquivalentStuffUsageResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int BillOfMaterialVersion { get; set; }
    public int EquivalentStuffId { get; set; }
    public string EquivalentStuffTitle { get; set; }
    public int EquivalentStuffStuffId { get; set; }
    public string EquivalentStuffCode { get; set; }
    public string EquivalentStuffName { get; set; }
    public double UsageQty { get; set; }
    public EquivalentStuffUsageStatus Status { get; set; }
    public int? ProductionPlanDetailId { get; set; }
    public string ProductionPlanDetailCode { get; set; }
    public int? ProductionPlanId { get; set; }
    public DateTime? ProductionPlanEstimatedDate { get; set; }
    public int? ProductionOrderId { get; set; }
    public string ProductionOrderCode { get; set; }
    public DateTime? ProductionOrderDateTime { get; set; }
    public EquivalentStuffType EquivalentStuffType { get; set; }
    public int BillOfMaterialDetailId { get; set; }
    public double BillOfMaterialDetailValue { get; set; }
    public int BillOfMaterialDetailUnitId { get; set; }
    public string BillOfMaterialDetailUnitName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
