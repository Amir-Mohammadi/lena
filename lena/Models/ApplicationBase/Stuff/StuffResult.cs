using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Stuff
{
  public class StuffResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Noun { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public int StuffCategoryId { get; set; }
    public string ParentStuffCategoryName { get; set; }
    public string CategoryName { get; set; }
    public byte UnitTypeId { get; set; }
    public string UnitTypeName { get; set; }
    public StuffType StuffType { get; set; }
    public int ProjectHeaderId { get; set; }
    public string ProjectHeaderName { get; set; }
    public int StockSafety { get; set; }
    public double FaultyPercentage { get; set; }
    public bool NeedToQualityControl { get; set; }
    public bool NeedToQualityControlDocumentUpload { get; set; }
    public bool IsTraceable { get; set; }
    public int? QualityControlDepartmentId { get; set; }
    public string QualityControlDepartmentName { get; set; }
    public int? QualityControlEmployeeId { get; set; }
    public string QualityControlEmployeeFullName { get; set; }
    public double Tolerance { get; set; }
    public double? Volume { get; set; }
    public double? NetWeight { get; set; }
    public double? GrossWeight { get; set; }
    public int? StuffHSGroupId { get; set; }
    public string StuffHSGroupCode { get; set; }
    public string StuffHSGroupTitle { get; set; }
    public int? StuffPurchaseCategoryId { get; set; }
    public string StuffPurchaseCategoryName { get; set; }
    public int? StuffDefinitionRequestId { get; set; }
    public int? QualityControlCheckDuration { get; set; }
    public byte[] RowVersion { get; set; }
    public bool CheckDocument { get; set; }
    public int? StuffPurchaseCategoryQualityControlDepartmentId { get; set; }
    public string StuffPurchaseCategoryQualityControlDepartmentName { get; set; }
    public short CeofficientSet { get; set; }
  }
}