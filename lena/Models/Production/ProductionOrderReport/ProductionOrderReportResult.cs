using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrderReport
{
  public class ProductionOrderReportResult
  {
    public int? Index { get; set; }
    public int? OperationCount { get; set; }
    public double BatchCount { get; set; }
    public long SwitchTime { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string ProductionStepName { get; set; }
    public string WorkPlanTitle { get; set; }
    public double Qty { get; set; }
    public string UnitName { get; set; }
    public double ProducedQty { get; set; }
    public string ProductionLineName { get; set; }
    public string ConsumeWarehouseName { get; set; }
    public string ProductWarehouseName { get; set; }
    public string OperationTitle { get; set; }
    public string OperatorTypeName { get; set; }
    public long DefaultTime { get; set; }
    public string MachineTypeOperatorTypeTitle { get; set; }
    public string EmployeeFullName { get; set; }
    public int? ProductionTerminalId { get; set; }
    public string ProductionTerminalDescription { get; set; }




  }
}
