using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingBlocker
{
  public class DeleteLadingChangeRequestResult
  {
    public int Id { get; set; }
    public int CargoItemId { get; set; }
    public int? LadingItemId { get; set; }
    public int LadingItemDetailId { get; set; }
    public int LadingChangeRequestId { get; set; }
    public byte[] LadingItemDetailRowVersion { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
