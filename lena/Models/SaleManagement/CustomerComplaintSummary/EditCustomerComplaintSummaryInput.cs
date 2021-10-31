using System;
using System.Collections.Generic;
using lena.Models.SaleManagement.CustomerComplaintDepartment;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaintSummary
{
  public class EditCustomerComplaintSummaryInput
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
    public Nullable<System.Guid> DocumentId { get; set; }
    public byte[] File { get; set; }
    public string ComplaintCorrectiveActionRegistrarName { get; set; }
    public DateTime? CorrectiveActionDateTime { get; set; }
    public int CorrectiveActionUserId { get; set; }
    public IEnumerable<CustomerComplaintDepartmentResult> CustomerComplaintDepartments { get; set; }
    public string DepartmentFullName { get; set; }
  }
}