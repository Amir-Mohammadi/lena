using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.SuppliesPurchaserUser
{
  public class GetSuppliesPurchaserUsersInput : SearchInput<SuppliesPurchaserUserSortType>
  {
    public int? StuffId { get; set; }
    public int? PurchaserUserId { get; set; }
    public bool? IsDefault { get; set; }
    public GetSuppliesPurchaserUsersInput(PagingInput pagingInput, SuppliesPurchaserUserSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
