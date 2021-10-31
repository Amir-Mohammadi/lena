using System.ComponentModel.DataAnnotations;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class FlatBillOfMaterialRawResult
  {
    [Key]
    public int Id { get; set; }
    public int Root { get; set; }
    public int Parent { get; set; }
    public int Child { get; set; }
    public int Version { get; set; }
    public double Value { get; set; }
    public string Type { get; set; }
    public int TypeId { get; set; }
    public byte UnitId { get; set; }
  }
}
