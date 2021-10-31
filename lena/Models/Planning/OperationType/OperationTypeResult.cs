using lena.Domains.Enums;
namespace lena.Models.Planning.OperationType
{
  public class OperationTypeResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public byte[] Symbol { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
