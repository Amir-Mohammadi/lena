using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Allocation
{
  public class GetAllocationInput : SearchInput<AllocationSortType>
  {
    public int? BankOrderId { get; set; }
    public GetAllocationInput(PagingInput pagingInput, AllocationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
      this.BankOrderId = null;
    }
  }
}

