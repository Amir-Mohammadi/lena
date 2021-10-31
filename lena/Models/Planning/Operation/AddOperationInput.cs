using lena.Domains.Enums;
namespace lena.Models.Planning.Operation
{
  public class AddOperationInput
  {
    public string Title { get; set; }
    public string Code { get; set; }
    public bool IsQualityControl { get; set; }
    public bool IsCorrective { get; set; }
    public byte OperationTypeId { get; set; }
    public string Description { get; set; }
  }
}
