using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingItemDetail
{
  public class EditLadingItemDetailInput
  {
    public int Id { get; set; }
    public int LadingItemId { get; set; }
    public int CargoItemId { get; set; }
    public int CargoItemDetailId { get; set; }
    public double Qty { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
