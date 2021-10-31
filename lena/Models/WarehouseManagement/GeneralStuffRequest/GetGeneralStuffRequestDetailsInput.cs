using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.GeneralStuffRequest
{
  public class GetGeneralStuffRequestDetailsInput : SearchInput<GeneralStuffRequestDetailSortType>
  {
    public GetGeneralStuffRequestDetailsInput(PagingInput pagingInput, GeneralStuffRequestDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? GeneralStuffRequestDetailId { get; set; }
    public int? GeneralStuffRequestId { get; set; }
    public int? StuffRequestId { get; set; }
    public int? PurchaseRequestId { get; set; }
    public int? AlternativePurchaseRequestId { get; set; }
    public int? StuffId { get; set; }
  }
}
