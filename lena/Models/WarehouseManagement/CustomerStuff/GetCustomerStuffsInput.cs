using lena.Models.Common;
using lena.Domains.Enums;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.CustomerStuff
{
  public class GetCustomerStuffsInput : SearchInput<CustomerStuffSortType>
  {
    public string Code { get; set; }
    public GetCustomerStuffsInput(PagingInput pagingInput, CustomerStuffSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

