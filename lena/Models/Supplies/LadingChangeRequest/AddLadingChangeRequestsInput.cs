using lena.Domains.Enums;
using lena.Models.Supplies.LadingItemDetail;

using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingChangeRequest
{
  public class AddLadingChangeRequestsInput
  {
    public int LadingId { get; set; }
    public int DemandantUserId { get; set; }
    public string Description { get; set; }
    public LadingType LadingType { get; set; }
    public AddLadingItemDetailInput[] AddLadingItemDetailChanges { get; set; }
    public EditLadingItemDetailInput[] EditLadingItemDetailChanges { get; set; }
    public DeleteLadingItemDetailInput[] DeleteLadingItemDetailChanges { get; set; }
  }
}
