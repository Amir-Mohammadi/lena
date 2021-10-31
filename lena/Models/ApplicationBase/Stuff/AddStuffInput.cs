using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Stuff
{
  public class AddStuffInput
  {
    public string Name { get; set; }
    public string Noun { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public short StuffCategoryId { get; set; }
    public byte UnitTypeId { get; set; }
    public StuffType StuffType { get; set; }
    public int StockSafety { get; set; }
    public double FaultyPercentage { get; set; }
    public bool NeedToQualityControl { get; set; }
    public bool NeedToQualityControlDocumentUpload { get; set; }
    public bool IsTraceable { get; set; }
    public short? QualityControlDepartmentId { get; set; }
    public int? QualityControlEmployeeId { get; set; }
    public double Tolerance { get; set; }
    public double? Volume { get; set; }
    public double? NetWeight { get; set; }
    public double? GrossWeight { get; set; }
    public int? StuffHSGroupId { get; set; }
    public int? StuffPurchaseCategoryId { get; set; }
    public int? StuffDefinitionRequestId { get; set; }
    public byte[] StuffDefinitionRequestRowVersion { get; set; }
    public int? QualityControlCheckDuration { get; set; }
    public short CeofficientSet { get; set; }

  }
}