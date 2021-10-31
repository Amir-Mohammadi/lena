
using lena.Domains.Enums;
using lena.Models.Production.RepairProductionFault;

using lena.Domains.Enums;
namespace lena.Models.Production.RepairProduction
{
  public class AddRepairProductionInput
  {
    public string Serial { get; set; }
    public RepairProductionSerialStatus SerialStatus { get; set; }
    public string InnerSerial { get; set; }
    public bool IsUnrepairable { get; set; }
    public int[] EmployeeIds { get; set; }
    public int[] FialedOperationIds { get; set; }
    public int[] ProductionFaultTypeIds { get; set; }
    public int ProductionTerminalId { get; set; }
    public bool IsDelete { get; set; }
    public string Description { get; set; }
    public AddRepairProductionFaultInput[] StuffDetails { get; set; }
    public FaultCauseDetail[] FaultCauseDetails { get; set; }
    public bool AutoTransferSerialToProductionLineConsumeWarehouse { get; set; }
    public int? ProductionLineId { get; set; }
  }



}
