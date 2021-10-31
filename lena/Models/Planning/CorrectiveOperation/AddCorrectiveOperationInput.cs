using lena.Domains.Enums;
namespace lena.Models.Planning.CorrectiveOperation
{
  public class AddCorrectiveOperationInput
  {
    public string Title { get; set; }
    public string Code { get; set; }
    public bool IsQualityControl { get; set; }
    public int OperationTypeId { get; set; }
    public string Description { get; set; }
  }
}
