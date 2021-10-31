using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionPlanDetail
{
  public class ProductionPlanDetailResult
  {
    public int Id { get; set; }
    public int ProductionPlanId { get; set; }
    public string ProductionPlanCode { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int BillOfMaterialVersion { get; set; }
    public int OrderItemId { get; set; }
    public string OrderItemCode { get; set; }
    public int? ProductionRequestId { get; set; }
    public DateTime ProductionRequestDeadlineDate { get; set; }
    public double ProductionPlanQty { get; set; }
    public int ProductionStepId { get; set; }
    public string ProductionStepName { get; set; }
    public int WorkPlanId { get; set; }
    public string WorkPlanTitle { get; set; }
    public int WorkPlanVersion { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double Qty { get; set; }
    public double ConversionRatio { get; set; }
    public double ScheduledQty { get; set; }
    public int DetailWorkPlanId { get; set; }
    public int SemiProductStuffId { get; set; }
    public string SemiProductStuffCode { get; set; }
    public string SemiProductStuffName { get; set; }
    public string ProductionRequestCode { get; set; }
    public DateTime OrderItemDeliveryDate { get; set; }
    public int ProductionPlanUnitId { get; set; }
    public double OrderItemPlannedQty { get; set; }
    public string ProductionPlanUnitName { get; set; }
    public int LevelId { get; set; }
    public int? ParentLevelId { get; set; }
    public DateTime EstimatedDate { get; set; }

    public int OrderId { get; set; }
    public double OrderItemQty { get; set; }
    public string OrderItemUnit { get; set; }
    public double ProductionRequestQty { get; set; }
    public string ProductionRequestUnit { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
