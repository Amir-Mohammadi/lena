using lena.Models.Planning.BillOfMaterialDetail;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class EditBillOfMaterialInput
  {
    public int StuffId { get; set; }
    public short Version { get; set; }
    public string Title { get; set; }
    public BillOfMaterialVersionType BillOfMaterialVersionType { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public int QtyPerBox { get; set; }
    public string Description { get; set; }
    public AddBillOfMaterialDetailInput[] AddBillOfMaterialDetails { get; set; }
    public EditBillOfMaterialDetailInput[] EditBillOfMaterialDetails { get; set; }
    public int[] DeleteBillOfMaterialDetails { get; set; }
    public byte[] RowVersion { get; set; }
    public int ProductionStepId { get; set; }
  }
}
