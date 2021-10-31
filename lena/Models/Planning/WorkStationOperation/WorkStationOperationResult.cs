using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStationOperation
{
  public class WorkStationOperationResult
  {
    public int OperationId { get; set; }
    public string OperationCode { get; set; }
    public string OperationTitle { get; set; }
    public int WorkStationId { get; set; }
    public string WorkStationName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
