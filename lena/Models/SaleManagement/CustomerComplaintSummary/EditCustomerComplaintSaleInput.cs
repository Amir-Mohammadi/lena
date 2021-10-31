using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaintSummary
{
  public class EditCustomerComplaintSaleInput
  {
    public int Id { get; set; }
    public Nullable<DateTime> DateOfAnnouncement { get; set; }
    public string CustomerOpinion { get; set; }

  }
}