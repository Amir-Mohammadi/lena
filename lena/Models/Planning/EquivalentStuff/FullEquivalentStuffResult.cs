using lena.Domains.Enums;
using lena.Models.Planning.EquivalentStuffDetail;

using lena.Domains.Enums;
namespace lena.Models.Planning.EquivalentStuff
{
  public class FullEquivalentStuffResult
  {

    public FullEquivalentStuffDetailResult[] EquivalentStuffDetails { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    //public BillOfMaterialDetail BillOfMaterialDetailId { get; set; }
    public EquivalentStuffType EquivalentStuffType { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
