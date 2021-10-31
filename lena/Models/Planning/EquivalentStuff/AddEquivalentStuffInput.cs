using lena.Domains.Enums;
using lena.Models.Planning.EquivalentStuffDetail;

using lena.Domains.Enums;
namespace lena.Models.Planning.EquivalentStuff
{
  public class AddEquivalentStuffInput
  {
    public int BillOfMaterialDetailId { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public EquivalentStuffType EquivalentStuffType { get; set; }
    public string Description { get; set; }
    public AddEquivalentStuffDetailInput[] EquivalentStuffDetails { get; set; }
  }
}
