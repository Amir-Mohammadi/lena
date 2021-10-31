using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingBlocker
{
  public class LadingBlockerResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int UserGroupId { get; set; }
    public string UserGroupName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
