using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CustomerComplaintSummary : IEntity
  {
    protected internal CustomerComplaintSummary()
    {
      this.CustomerComplaintDepartments = new HashSet<CustomerComplaintDepartment>();
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string ComplaintClassificationTypeDescription { get; set; }
    public Nullable<ComplaintClassificationTypes> ComplaintClassificationTypes { get; set; }
    public ComplaintStatus Status { get; set; }
    public string QAOpinion { get; set; }
    public string CustomerOpinion { get; set; }
    public Nullable<DateTime> DateOfAnnouncement { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public Nullable<Guid> CorrectiveActionDocumentId { get; set; }
    public int CustomerComplaintId { get; set; }
    public OccurrenceSeverityStatus OccurrenceSeverityStatus { get; set; }
    public OccurrenceProbabilityStatus OccurrenceProbabilityStatus { get; set; }
    public Nullable<RiskLevelStatus> RiskLevelStatus { get; set; }
    public string ComplaintTitle { get; set; }
    public string CorrectiveAction { get; set; }
    public Nullable<DateTime> CorrectiveActionDateTime { get; set; }
    public Nullable<int> CorrectiveActionUserId { get; set; }
    public virtual User CorrectiveActionUser { get; set; }
    public virtual CustomerComplaint CustomerComplaint { get; set; }
    public virtual ICollection<CustomerComplaintDepartment> CustomerComplaintDepartments { get; set; }
  }
}