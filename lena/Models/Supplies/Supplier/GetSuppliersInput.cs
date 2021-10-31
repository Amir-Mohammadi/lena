using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Supplier
{
  public class GetSuppliersInput : SearchInput<SupplierSortType>
  {
    public GetSuppliersInput(PagingInput pagingInput, SupplierSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? EmployeeId { get; set; }
    public string Description { get; set; }
    public bool? IsActive { get; set; }
  }
}
