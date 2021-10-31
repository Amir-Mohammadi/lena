using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperatorMachineEmployee
{
  public class EditProductionOperatorMachineEmployeeInput
  {
    public int Id { get; set; }
    public int? EmployeeId { get; set; }
    public short? MachineId { get; set; }
    public int? ProductionTerminalId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
