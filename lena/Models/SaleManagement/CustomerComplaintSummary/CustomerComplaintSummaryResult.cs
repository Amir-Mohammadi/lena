using System;
using System.Collections.Generic;
using System.Linq;
using lena.Models.SaleManagement.CustomerComplaintDepartment;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaintSummary
{
  public class CustomerComplaintSummaryResult
  {
    public int Id { get; set; }
    public string InhibitionAction { get; set; }
    public string CustomerOpinion { get; set; }
    public string QAOpinion { get; set; }
    public Nullable<DateTime> DateOfAnnouncement { get; set; }
    public Nullable<System.Guid> DocumentId { get; set; }
    public Nullable<System.Guid> CorrectiveActionDocumentId { get; set; }
    public Nullable<ComplaintClassificationTypes> ComplaintClassificationTypes { get; set; }
    public Nullable<ComplaintStatus> Status { get; set; }
    public IQueryable<short> SelectedDepartmentIds { get; set; }
    public string FileKey { get; set; }
    public byte[] File { get; set; }
    public string ComplaintClassificationTypeDescription { get; set; }
    public OccurrenceSeverityStatus OccurrenceSeverityStatus { get; set; }
    public OccurrenceProbabilityStatus OccurrenceProbabilityStatus { get; set; }
    public CustomerComplaintType CustomerComplaintType { get; set; }
    public RiskLevelStatus RiskLevelStatus { get; set; }
    public string ComplaintTitle { get; set; }
    public string CorrectiveAction { get; set; }
    public string ComplaintCorrectiveActionRegistrarName { get; set; }
    public IEnumerable<CustomerComplaintDepartmentResult> CustomerComplaintDepartments { get; set; }
    public string DepartmentFullName { get; set; }
  }
}
