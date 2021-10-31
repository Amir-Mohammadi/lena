using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class BillOfMaterialComparisonResult
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffNoun { get; set; }
    public string StuffName { get; set; }
    public int? Version1 { get; set; }
    public double? Value1 { get; set; }
    public int? Version2 { get; set; }
    public double? Value2 { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }

    public BillOfMaterialComparisonStatus Status
    {
      get
      {
        BillOfMaterialComparisonStatus? status = null;
        if (Value1 == null)
          status = BillOfMaterialComparisonStatus.IsAdded;
        else if (Value2 == null)
          status = BillOfMaterialComparisonStatus.IsDeleted;
        else if (Value2 != Value1)
          status = BillOfMaterialComparisonStatus.HasChanged;
        else
          status = BillOfMaterialComparisonStatus.NoChange;
        return status.Value;
      }
    }
  }
}

