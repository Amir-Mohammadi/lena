using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingChangeRequest
{
  public class RejectLadingChangeRequestInput
  {
    public int Id { get; set; }
    public int LadingId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
