using lena.Domains.Enums;
using lena.Models.Planning.EquivalentStuffDetail;

using lena.Domains.Enums;
namespace lena.Models.Planning.EquivalentStuff
{
  public class EditEquivalentStuffInput
  {
    public int Id { get; set; }
    public int BillOfMaterialDetailId { get; set; }
    public string Title { get; set; }
    public EquivalentStuffType EquivalentStuffType { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public AddEquivalentStuffDetailInput[] AddEquivalentStuffDetails { get; set; }
    public EditEquivalentStuffDetailInput[] EditEquivalentStuffDetails { get; set; }
    public int[] DeleteEquivalentStuffDetails { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
