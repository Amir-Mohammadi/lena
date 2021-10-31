using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;


using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaintSummary
{
  public class GetCustomerComplaintSummariesInput : SearchInput<CustomerComplaintSortType>
  {
    public int Id { get; set; }
    public GetCustomerComplaintSummariesInput(PagingInput pagingInput, CustomerComplaintSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
