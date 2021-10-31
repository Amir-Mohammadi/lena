using lena.Domains.Enums;
namespace lena.Models.Planning.Operation
{
  public class OperationComboResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public bool IsQualityControl { get; set; }
    public byte[] OperationTypeSymbol { get; set; }
  }
}
