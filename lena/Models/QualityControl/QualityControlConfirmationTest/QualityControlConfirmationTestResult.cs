using lena.Domains.Enums;
using lena.Models.QualityControl.QualityControlConfirmationTestItem;
using lena.Models.QualityControl.StuffQualityControlTestCondition;
using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmationTest
{
  public class QualityControlConfirmationTestResult
  {
    public int Id { get; set; }
    public long QualityControlTestId { get; set; }
    public string QualityControlTestCode { get; set; }
    public string QualityControlTestName { get; set; }
    public string QualityControlTestDescription { get; set; }
    public int QualityControlConfirmationId { get; set; }
    public QualityControlConfirmationTestStatus Status { get; set; }
    public double? TestedQty { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }

    public IQueryable<QualityControlConfirmationTestItemResult> QualityControlConfirmationTestItems { get; set; }
  }

  public class FullQualityControlConfirmationTestResult : QualityControlConfirmationTestResult
  {
    public int QualityControlId { get; set; }
    public string QualityControlCode { get; set; }
    public double? Min { get; set; }
    public double? Max { get; set; }
    public string TesterFullName { get; set; }
    public int? QualityControlTestUnitId { get; set; }
    public string QualityControlTestUnitName { get; set; }


    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public Guid? DocumentId { get; set; }
    public int? QualityControlTestConditionId { get; set; }
    public string QualityControlTestCondition { get; set; }
    public ToleranceType? ToleranceType { get; set; }
    public string AcceptanceLimit { get; set; }
    public double AQLAmount { get; set; }
    public string SampleName { get; set; }
    public int TesterUserId { get; set; }
    public int? TestConditionId { get; set; }
    public int? QualityControlTestConditionTestConditionId { get; set; }
    public string Condition { get; set; }
    public IQueryable<QualityControlConfirmationTestItemResult> QualityControlConfirmationTestItems { get; set; }
    public IQueryable<StuffQualityControlTestConditionResult> StuffQualityControlTestConditions { get; set; }
  }
}
