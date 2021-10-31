using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperatorMachineEmployee
{
  public class ProductionOperatorMachineEmployeeResult
  {
    public int Id { get; set; }
    public int Index { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public int? MachineId { get; set; }
    public string MachineName { get; set; }
    public int ProductionOperatorId { get; set; }
    public int? ProductionTerminalId { get; set; }
    public string ProductionTerminalDescription { get; set; }
    public string Description { get; set; }
    public bool? IsBan { get; set; }
    public int? LatestBanId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
