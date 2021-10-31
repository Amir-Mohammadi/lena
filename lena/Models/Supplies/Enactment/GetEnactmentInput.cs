using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Enactment
{
  public class GetEnactmentsInput : SearchInput<EnactmentSortType>
  {
    public int? BankOrderId { get; set; }
    public GetEnactmentsInput(PagingInput pagingInput, EnactmentSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
      this.BankOrderId = null;
    }
  }
}

