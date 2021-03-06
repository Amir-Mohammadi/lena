using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingItemDetail
{
  public class EditLadingChangeRequestResult
  {
    public int Id { get; set; }
    public double Qty { get; set; }
    public int LadingItemId { get; set; }
    public int CargoItemId { get; set; }
    public int CargoItemDetailId { get; set; }
    public int LadingItemDetailId { get; set; }
    public int LadingChangeRequestId { get; set; }
    public byte[] LadingItemDetailRowVersion { get; set; }
    public byte[] RowVersion { get; set; }

  }
}