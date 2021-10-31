using System;
using System.Collections.Generic;
using System.Linq;
using lena.Models.SaleManagement.CustomerComplaintDepartment;
using lena.Domains.Enums;
using lena.Models.SaleManagement.CustomerComplaintSummary;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaint
{
  public class CustomerComplaintResult
  {
    public int Id { get; set; }
    public Nullable<int> CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string ComplaintRegistrarName { get; set; }
    public Nullable<DateTime> DateOfComplaint { get; set; }
    public Nullable<DateTime> ResponseDeadline { get; set; }
    public Nullable<DateTime> RegisterDateTime { get; set; }
    public ComplaintTypes ComplaintTypes { get; set; }
    public string ComplaintTypeDescription { get; set; }
    public int? DepartmentCount { get; set; }
    public IEnumerable<CustomerComplaintSummaryResult> CustomerComplaintSummaries { get; set; }
    public string InhibitionAction { get; set; }
    public string CustomerOpinion { get; set; }
    public string QAOpinion { get; set; }
    public Nullable<DateTime> DateOfAnnouncement { get; set; }
    public Nullable<System.Guid> DocumentId { get; set; }
    public Nullable<ComplaintClassificationTypes> ComplaintClassificationTypes { get; set; }
    public Nullable<ComplaintStatus> Status { get; set; }
    public int[] SelectedDepartmentIds { get; set; }
    public string FileKey { get; set; }
    public byte[] File { get; set; }
    public string ComplaintClassificationTypeDescription { get; set; }
    public OccurrenceSeverityStatus OccurrenceSeverityStatus { get; set; }
    public OccurrenceProbabilityStatus OccurrenceProbabilityStatus { get; set; }
    public CustomerComplaintType CustomerComplaintType { get; set; }
    public Nullable<RiskLevelStatus> RiskLevelStatus { get; set; }
    public string ComplaintTitle { get; set; }
    public string CorrectiveAction { get; set; }
    public string ComplaintCorrectiveActionRegistrarName { get; set; }
    public string DepartmentName
    {
      get
      {
        return CustomerComplaintDepartments != null
            ? string.Join(" ، ", CustomerComplaintDepartments.Select(i => i.DepartmentName).Distinct())
            : "";
      }
    }
    public IEnumerable<CustomerComplaintDepartmentResult> CustomerComplaintDepartments { get; set; }
    public string DepartmentFullName { get; set; }
    public int CustomerComplaintSummaryId { get; set; }
    public int DepartmentId { get; set; }
    public Nullable<System.DateTime> DateOfInhibition { get; set; }
  }
}
