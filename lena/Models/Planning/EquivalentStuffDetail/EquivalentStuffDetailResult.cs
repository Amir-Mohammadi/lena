using lena.Domains.Enums;
namespace lena.Models.Planning.EquivalentStuffDetail
{
  public class EquivalentStuffDetailResult
  {
    public byte[] RowVersion { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public int EquivalentStuffDetailId { get; set; }
  }
}
