using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaintDepartment
{
  public class EditCustomerComplaintDepartmentInput
  {
    public int Id { get; set; }
    public string InhibitionAction { get; set; }
    public Nullable<DateTime> DateOfInhibition { get; set; }
  }
}