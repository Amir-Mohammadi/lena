using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingItemDetail
{
  public class GetLadingItemDetailsInput : SearchInput<LadingItemDetailSortType>
  {
    public int? StuffId { get; set; }
    public int? CooperatorId { get; set; }
    public bool? HasReceiptLicence { get; set; }

    public GetLadingItemDetailsInput(PagingInput pagingInput, LadingItemDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
