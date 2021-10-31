using lena.Models.Production.ProductionOperatorMachineEmployee;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperator
{
  public class AddProductionOperatorInput
  {
    public int ProductionOrderId { get; set; }
    public int WrongLimitCount { get; set; }
    public int? OperationSequenceId { get; set; }
    public short OperationId { get; set; }
    public short? MachineTypeOperatorTypeId { get; set; }
    public short? OperatorTypeId { get; set; }
    public int DefaultTime { get; set; }
    //public int? ProductionTerminalId { get; set; }

    public AddProductionOperatorMachineEmployeeInput[] AddProductionOperatorMachineEmployees;
  }
}
