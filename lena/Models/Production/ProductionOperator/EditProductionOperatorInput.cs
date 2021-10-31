using lena.Models.Production.ProductionOperatorMachineEmployee;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperator
{
  public class EditProductionOperatorInput
  {
    public int Id { get; set; }
    public int? OperationSequenceId { get; set; }
    public short OperationId { get; set; }
    public int DefaultTime { get; set; }
    public short? MachineTypeOperatorTypeId { get; set; }
    public short? OperatorTypeId { get; set; }
    //public int? ProductionTerminalId { get; set; }
    public int WrongLimitCount { get; set; }
    public byte[] RowVersion { get; set; }
    public AddProductionOperatorMachineEmployeeInput[] AddProductionOperatorMachineEmployees { get; set; }
    public EditProductionOperatorMachineEmployeeInput[] EditProductionOperatorMachineEmployees { get; set; }
    public DeleteProductionOperatorMachineEmployeeInput[] DeleteProductionOperatorMachineEmployees { get; set; }
  }
}
