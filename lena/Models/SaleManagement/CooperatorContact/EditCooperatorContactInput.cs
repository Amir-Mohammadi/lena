using lena.Domains.Enums;
namespace lena.Models
{
  public class EditCooperatorContactInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string ContactText { get; set; }
    public int ContactTypeId { get; set; }
    public bool IsMain { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
