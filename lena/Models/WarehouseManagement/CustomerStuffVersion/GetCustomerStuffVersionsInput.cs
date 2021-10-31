using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.CustomerStuffVersion
{
  public class GetCustomerStuffVersionsInput : SearchInput<CustomerStuffVersionSortType>
  {
    public int? CustomerStuffId { get; set; }
    public bool? IsPublish { get; set; }
    public GetCustomerStuffVersionsInput(PagingInput pagingInput, CustomerStuffVersionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

