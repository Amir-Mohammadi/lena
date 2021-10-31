using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  public class GetRialInvoicesInput : SearchInput<RialInvoiceSortType>
  {
    public int? StuffId { get; set; }
    public int? SourceCurrencyId { get; set; }
    public int? UserId { get; set; }

    public GetRialInvoicesInput(PagingInput pagingInput, RialInvoiceSortType sortType, SortOrder sortOrder) :
        base(pagingInput: pagingInput, sortType: sortType, sortOrder: sortOrder)
    {
    }
  }
}
