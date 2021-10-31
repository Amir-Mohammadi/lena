using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingItem
{
  public class EditLadingItemInput
  {
    public int Id { get; set; }

    public int CargoItemId { get; set; }

    public double Qty { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
