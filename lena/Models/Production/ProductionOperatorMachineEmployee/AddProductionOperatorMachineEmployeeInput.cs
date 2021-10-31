using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperatorMachineEmployee
{
  public class AddProductionOperatorMachineEmployeeInput
  {
    public int ProductionOperatorId { get; set; }
    public int? EmployeeId { get; set; }
    public short? MachineId { get; set; }
    public int? ProductionTerminalId { get; set; }
    public string Description { get; set; }
  }
}
