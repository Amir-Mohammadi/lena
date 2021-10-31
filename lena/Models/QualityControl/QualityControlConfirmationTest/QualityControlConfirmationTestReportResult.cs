using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmationTest
{
  public class QualityControlConfirmationTestReportResult
  {
    public int QualityControlConfirmationTestId { get; set; }
    public int QualityControlConfirmationId { get; set; }
    public int QualityControlId { get; set; }
    public string QualityControlTestCode { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double EntryQty { get; set; }
    public DateTime EnterTime { get; set; }
    public DateTime? ConfirmationTime { get; set; }
    public int? ConfirmationUserId { get; set; }
    public int? ConfirmationEmployeeId { get; set; }
    public string ConfirmationEmployeeFullName { get; set; }
    public double SampleQty { get; set; }
    public long QualityControlTestId { get; set; }
    public string QualityControlTestName { get; set; }
    public string QualityControlTestDescription { get; set; }
    public double? Min { get; set; }
    public double? Max { get; set; }
    public double? TestedQty { get; set; }
    public QualityControlConfirmationTestStatus Status { get; set; }
    public int? QualityControlTestUnitId { get; set; }
    public string QualityControlTestUnitName { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public Guid? DocumentId { get; set; }
  }
}