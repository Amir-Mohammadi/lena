using System;
using lena.Domains.Enums;
using lena.Models.SaleManagement.CustomerComplaintSummary;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaint
{
  public class EditCustomerComplaintInput
  {
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Nullable<DateTime> DateOfComplaint { get; set; }
    public Nullable<DateTime> ResponseDeadline { get; set; }
    public ComplaintTypes[] ComplaintTypes { get; set; }
    public string ComplaintTypeDescription { get; set; }
    public AddCustomerComplaintSummaryInput[] AddCustomerComplaintSummaryInput { get; set; }
    public EditCustomerComplaintSummaryInput[] EditCustomerComplaintSummaryInput { get; set; }
    public int[] DeleteCustomerComplaintSummaryInput { get; set; }
    public CustomerComplaintType CustomerComplaintType { get; set; }
  }
}