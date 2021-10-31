using lena.Domains.Enums;
namespace lena.Models.Production.StuffProductionFaultType
{
  public class StuffProductionFaultTypeComboResult
  {
    public int ProductionFaultTypeId { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string FaultName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
