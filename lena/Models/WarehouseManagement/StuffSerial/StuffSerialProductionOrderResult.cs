using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  public class StuffSerialProductionOrderResult
  {
    public int ProductionOrderId { get; set; }
    public string ProductionOrderCode { get; set; }
    public string WorkPlanTitle { get; set; }
    public int WorkPlanVersion { get; set; }
    public string SupervisorEmployeeFullName { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime ToDateTime { get; set; }

  }
}
