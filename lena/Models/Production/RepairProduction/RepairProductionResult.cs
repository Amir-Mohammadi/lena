using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Production.RepairProduction
{
  public class RepairProductionResult
  {
    public int Id { get; set; }
    public string Serial { get; set; }
    public RepairProductionSerialStatus SerialStatus { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public int StuffId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string BillOfMaterialVersionTitle { get; set; }
    public int WorkPlanId { get; set; }
    public string WorkPlanTitle { get; set; }
    public DateTime DateTime { get; set; }
    public string EmployeeFullName { get; set; }
    public int ProductionTerminalId { get; set; }
    public string ProductionTerminalTitle { get; set; }
    public byte[] RowVersion { get; set; }
    public RepairProductionStatus Status { get; set; }
  }
}