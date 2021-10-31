using System;
using lena.Domains.Enums;
using lena.Models.SaleManagement.CustomerComplaintSummary;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaint
{
  public class AddCustomerComplaintInput
  {
    public int CustomerId { get; set; }
    public Nullable<DateTime> DateOfComplaint { get; set; }
    public Nullable<DateTime> ResponseDeadline { get; set; }
    public ComplaintTypes[] ComplaintTypes { get; set; }
    public string ComplaintTypeDescription { get; set; }
    public AddCustomerComplaintSummaryInput[] CustomerComplaintSummaryInput { get; set; }
    public CustomerComplaintType CustomerComplaintType { get; set; }

  }
}