using lena.Domains.Enums;
namespace lena.Models.Planning.OperationType
{
  public class EditOperationTypeInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string ImageKey { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
