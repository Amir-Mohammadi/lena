using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingBlocker
{
  public class EditLadingBlockerInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int UserGroupId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
