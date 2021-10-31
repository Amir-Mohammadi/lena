using lena.Domains.Enums;
using lena.Models.QualityControl.StuffQualityControlTestCondition;
using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmationTestItem
{
  public class QualityControlConfirmationTestItemResult
  {
    public int? Id { get; set; }
    public string SampleName { get; set; }
    public string TesterEmployeeFullName { get; set; }
    public int? TesterUserId { get; set; }
    public int? UserId { get; set; }
    public string QualityControlConfirmationTestEmployeeFullName { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public long QualityControlTestId { get; set; }
    public string QualityControlTestName { get; set; }
    public double AQLAmount { get; set; }
    public double? MaxObtainAmount { get; set; }
    public double? MinObtainAmount { get; set; }
    public double? ObtainAmount { get; set; }
    public byte[] RowVersion { get; set; }
    public DateTime? QualityControlConfirmationTestDateTime { get; set; }

    public int? QualityControlId { get; set; }
    public string QualityControlCode { get; set; }

    public IQueryable<StuffQualityControlTestConditionResult> StuffQualityControlTestConditions { get; set; }
    public QualityControlConfirmationTestStatus QualityControlConfirmationTestStatus { get; set; }
    public int? TestConditionId { get; set; }
    public string Condition { get; set; }

  }
}
