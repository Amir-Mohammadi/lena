using lena.Models.QualityControl.StuffQualityControlTestCondition;
using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffQualityControlTest
{
  public class StuffQualityControlTestResult
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public long QualityControlTestId { get; set; }
    public string QualityControlTestCode { get; set; }
    public string QualityControlTestName { get; set; }
    public string QualityControlTestDescription { get; set; }
    public string MeasurementMethod { get; set; }
    public string CorrectiveReaction { get; set; }
    public string Frequency { get; set; }
    public byte[] RowVersion { get; set; }
    public Guid? DocumentId { get; set; }
    public int? QualityControlTestConditionId { get; set; }
    public int? QualityControlTestEquipmentId { get; set; }
    public int? QualityControlTestOperationId { get; set; }
    public int? QualityControlTestImportanceDegreeId { get; set; }
    public IQueryable<StuffQualityControlTestConditionResult> StuffQualityControlTestConditionResult { get; set; }
    public IQueryable<StuffQualityControlTestEquipmentResult> StuffQualityControlTestEquipmentResult { get; set; }
    public IQueryable<StuffQualityControlTestImportanceDegreeResult> StuffQualityControlTestImportanceDegreeResult { get; set; }
    public IQueryable<StuffQualityControlTestOperationResult> StuffQualityControlTestOperationResult { get; set; }
  }
}
