using lena.Domains.Enums;
namespace lena.Models.Supplies.Country
{
  public class EditCountryInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
