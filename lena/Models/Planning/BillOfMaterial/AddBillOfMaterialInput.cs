using lena.Models.Planning.BillOfMaterialDetail;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class AddBillOfMaterialInput
  {

    public int StuffId { get; set; }
    public string Title { get; set; }
    public BillOfMaterialVersionType BillOfMaterialVersionType { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public int QtyPerBox { get; set; }
    public int ProductionStepId { get; set; }
    public string Description { get; set; }
    public bool CopyDocuments { get; set; }
    public int? SourceVersionToCopyDocuments { get; set; }
    public AddBillOfMaterialDetailInput[] BillOfMaterialDetails { get; set; }
  }
}
