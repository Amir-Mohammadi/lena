using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class ContactTypeResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public EssentialContactType? EssentialContactType { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
