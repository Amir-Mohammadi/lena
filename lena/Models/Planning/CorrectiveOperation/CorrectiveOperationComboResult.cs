using lena.Domains.Enums;
namespace lena.Models.Planning.CorrectiveOperation
{
  public class CorrectiveOperationComboResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public byte[] OperationTypeSymbol { get; set; }
  }
}
