using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingCustomhouseStatus
{
  public class EditLadingCustomhouseStatusInput
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }

    public byte[] RowVersion { get; set; }
  }
}
