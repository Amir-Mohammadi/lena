using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class GetDistributedCostsAndDiscountsInput : SearchInput<DistributedCostsAndDiscountsSortType>
  {
    public int? FinancialDocumentId { get; set; }
    public int? StoreReceiptId { get; set; }


    public GetDistributedCostsAndDiscountsInput(PagingInput pagingInput, DistributedCostsAndDiscountsSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
