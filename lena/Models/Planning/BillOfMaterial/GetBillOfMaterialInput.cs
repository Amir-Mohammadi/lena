using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class GetBillOfMaterialInput
  {
    public int StuffId { get; set; }
    public int DetailStuffId { get; set; }
    public short Version { get; set; }

  }

  public class GetFlatBillOfMaterialInput : GetBillOfMaterialInput
  {
    public bool AddSemiProductToResult { get; set; }

  }
}
