using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingItemDetail
{
  public class DeleteLadingItemDetailInput
  {
    public int Id { get; set; }
    public int CargoItemId { get; set; }
    public int LadingItemId { get; set; }
    public int LadingItemDetailId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
