using lena.Domains.Enums;
namespace lena.Models.Production.StuffProductionFaultType
{
  public class StuffProductionFaultTypeResult
  {
    public int ProductionFaultTypeId { get; set; }
    public int StuffId { get; set; }
    public int? OperationId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string ProductionFaultTypeTitle { get; set; }
    public string OperationCode { get; set; }
    public string OperationTitle { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
