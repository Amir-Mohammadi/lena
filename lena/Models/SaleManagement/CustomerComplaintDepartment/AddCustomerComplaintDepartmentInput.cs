using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaintDepartment
{
  public class AddCustomerComplaintDepartmentInput
  {
    public short DepartmentId { get; set; }
    public int CustomerComplaintSummaryId { get; set; }
  }
}