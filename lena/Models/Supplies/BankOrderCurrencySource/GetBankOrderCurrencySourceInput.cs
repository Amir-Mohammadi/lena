using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrderCurrencySource
{
  public class GetBankOrderCurrencySourceInput : SearchInput<BankOrderCurrencySourceSortType>
  {
    public int? BankOrderId { get; set; }
    public int? LadingId { get; set; }
    public GetBankOrderCurrencySourceInput(PagingInput pagingInput, BankOrderCurrencySourceSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
      this.BankOrderId = null;
      this.LadingId = null;
    }
  }
}

