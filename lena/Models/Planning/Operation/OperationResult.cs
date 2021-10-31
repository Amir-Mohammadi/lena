using lena.Domains.Enums;
namespace lena.Models.Planning.Operation
{
  public class OperationResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public bool IsQualityControl { get; set; }
    public bool IsCorrective { get; set; }
    public int OperationTypeId { get; set; }
    public string OperationTypeTitle { get; set; }
    public byte[] OperationTypeSymbol { get; set; }
    public string Description { get; set; }
    public string Barcode { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
