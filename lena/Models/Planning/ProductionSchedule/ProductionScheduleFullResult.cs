using System;
using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionSchedule
{
  public class ProductionScheduleFullResult
  {
    public int Id { get; set; }
    public int TransactionBatchId { get; set; }
    public int ProductionPlanDetailId { get; set; }
    public int ProductionPlanId { get; set; }
    public string ProductionPlanCode { get; set; }
    public double Qty { get; set; }
    public bool ApplySwitchTime { get; set; }
    public int SwitchTime { get; set; }
    public int? ProductionRequestId { get; set; }
    public int OrderItemId { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public int OrderId { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public int ProductStuffId { get; set; }
    public string ProductStuffCode { get; set; }
    public string ProductStuffName { get; set; }
    public string ProductStuffNoun { get; set; }
    public double OrderItemQty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public string Code { get; set; }
    public int OrderItemUnitId { get; set; }
    public string OrderItemUnitName { get; set; }
    public DateTime DateTime { get; set; }

    public long Duration { get; set; }
    public int WorkPlanId { get; set; }
    public int BillOfMaterialVersion { get; set; }
    public int WorkPlanStepId { get; set; }
    public int ProductionLineId { get; set; }
    public int ProductionStepId { get; set; }
    public DateTime ToDateTime => DateTime.AddSeconds(Duration);
    public byte[] RowVersion { get; set; }
    public int StuffId { get; set; }
    public int ProductBillOfMaterialVersion { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string ProductionRequestCode { get; set; }
    public string OrderItemCode { get; set; }
  }
}
