using lena.Domains.Enums;
namespace lena.Models.Planning.CorrectiveOperation
{
  public class EditCorrectiveOperationInput
  {
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public bool IsQualityControl { get; set; }
    public string Description { get; set; }
  }
}
