using lena.Domains.Enums;
namespace lena.Models
{
  public class EditCustomhouseInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
