using lena.Domains.Enums;
namespace lena.Models
{
  public class AddCooperatorContactInput
  {
    public string Title { get; set; }
    public string ContactText { get; set; }
    public bool IsMain { get; set; }
    public int ContactTypeId { get; set; }
    public int CooperatorId { get; set; }
  }
}
