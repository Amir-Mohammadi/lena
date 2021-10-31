using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaintDepartment
{
  public class CustomerComplaintDepartmentResult
  {
    public int Id { get; set; }
    public int CustomerComplaintSummaryId { get; set; }
    public int DepartmentId { get; set; }
    public string InhibitionAction { get; set; }
    public Nullable<System.DateTime> DateOfInhibition { get; set; }
    public string DepartmentName { get; set; }
  }
}
