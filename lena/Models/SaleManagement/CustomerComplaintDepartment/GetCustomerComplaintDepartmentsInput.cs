using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;


using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaintDepartment
{
  public class GetCustomerComplaintDepartmentsInput : SearchInput<CustomerComplaintDepartmentSortType>
  {
    public int CustomerComplaintSummaryId { get; set; }
    public GetCustomerComplaintDepartmentsInput(PagingInput pagingInput, CustomerComplaintDepartmentSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}