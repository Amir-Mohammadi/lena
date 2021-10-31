using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Stuff
{
  public class StuffComboWithStuffTypeResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Noun { get; set; }
    public StuffType StuffType { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
