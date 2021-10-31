using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CustomerComplaint : IEntity
  {
    protected internal CustomerComplaint()
    {
      this.CustomerComplaintSummaries = new HashSet<CustomerComplaintSummary>();
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Code { get; set; }
    public Nullable<int> CustomerId { get; set; }
    public Nullable<DateTime> DateOfComplaint { get; set; }
    public Nullable<DateTime> ResponseDeadline { get; set; }
    public string ComplaintTypeDescription { get; set; }
    public ComplaintTypes ComplaintTypes { get; set; }
    public Nullable<int> RegisterarUserId { get; set; }
    public Nullable<DateTime> RegisterarDateTime { get; set; }
    public CustomerComplaintType CustomerComplaintType { get; set; }
    public virtual User RegisterarUser { get; set; }
    public virtual Cooperator Customer { get; set; }
    public virtual ICollection<CustomerComplaintSummary> CustomerComplaintSummaries { get; set; }
  }
}