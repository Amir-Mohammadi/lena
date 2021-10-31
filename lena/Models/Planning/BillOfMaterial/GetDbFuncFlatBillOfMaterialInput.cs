using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class GetDbFuncFlatBillOfMaterialInput
  {
    public int? RootId { get; set; }
    public int? ParentId { get; set; }
    public int? ChildId { get; set; }
    public int? Version { get; set; }
    public StuffType? StuffType { get; set; }
  }
}
