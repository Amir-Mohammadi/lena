using lena.Domains.Enums;
namespace lena.Models.Stuff
{
  public class StuffComboResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Noun { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
