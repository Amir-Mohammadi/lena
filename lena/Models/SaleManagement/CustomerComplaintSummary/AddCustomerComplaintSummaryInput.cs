using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaintSummary
{
  public class AddCustomerComplaintSummaryInput
  {
    public int Id { get; set; }
    public ComplaintClassificationTypes ComplaintClassificationTypes { get; set; }
    public ComplaintStatus Status { get; set; }
    public string QAOpinion { get; set; }
    public string CorrectiveAction { get; set; }
    public string CustomerOpinion { get; set; }
    public Nullable<DateTime> DateOfAnnouncement { get; set; }
    public string FileKey { get; set; }
    public string ComplaintClassificationTypeDescription { get; set; }
    public short[] SelectedDepartmentIds { get; set; }
    public OccurrenceSeverityStatus OccurrenceSeverityStatus { get; set; }
    public OccurrenceProbabilityStatus OccurrenceProbabilityStatus { get; set; }
    public string ComplaintTitle { get; set; }
    public RiskLevelStatus RiskLevelStatus { get; set; }
    public CustomerComplaintType CustomerComplaintType { get; set; }
  }
}