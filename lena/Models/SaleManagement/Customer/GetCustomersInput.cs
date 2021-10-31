using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.Customer
{
  public class GetCustomersInput : SearchInput<CustomerSortType>
  {
    public GetCustomersInput(PagingInput pagingInput, CustomerSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
