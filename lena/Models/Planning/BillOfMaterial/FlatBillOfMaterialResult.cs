using lena.Domains.Enums;
using System.ComponentModel.DataAnnotations;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class FlatBillOfMaterialResult
  {
    [Key]
    public int Id { get; set; }
    public int RootId { get; set; }
    public string RootCode { get; set; }
    public string RootName { get; set; }
    public int ParentId { get; set; }
    public string ParentCode { get; set; }
    public string ParentName { get; set; }
    public int ChildId { get; set; }
    public string ChildCode { get; set; }
    public string ChildName { get; set; }
    public int Version { get; set; }
    public double Value { get; set; }
    public StuffType StuffType { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
  }
}
