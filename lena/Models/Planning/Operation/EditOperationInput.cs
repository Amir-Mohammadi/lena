using lena.Domains.Enums;
namespace lena.Models.Planning.Operation
{
  public class EditOperationInput
  {

    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
    public bool IsQualityControl { get; set; }
    public bool IsCorrective { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
  }
}
