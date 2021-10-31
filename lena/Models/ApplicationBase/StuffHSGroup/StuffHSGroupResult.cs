using lena.Domains.Enums;
namespace lena.Models.StuffHSGroup
{
  public class StuffHSGroupResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}