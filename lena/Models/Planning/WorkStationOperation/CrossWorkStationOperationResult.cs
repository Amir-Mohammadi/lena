using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStationOperation
{
  public class CrossWorkStationOperationResult
  {
    public bool IsExist { get; set; }
    public short OperationId { get; set; }
    public string OperationCode { get; set; }
    public string OperationTitle { get; set; }
    public short WorkStationId { get; set; }
    public string WorkStationName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
