using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PriceInquiry
{
  public class GetPriceInquiriesInput : SearchInput<PriceInquirySortType>
  {
    public int? Id { get; set; }
    public int? StuffId { get; set; }
    public int? CooperatorId { get; set; }
    public byte? CurrencyId { get; set; }
    public int? EmployeeId { get; set; }
    public GetPriceInquiriesInput(PagingInput pagingInput, PriceInquirySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
