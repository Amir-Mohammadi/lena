using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PriceAnnunciation
{
  public class GetPriceAnnunciationInput : SearchInput<PriceAnnunciationSortType>
  {

    public int Id { get; set; }
    public DateTime? ValidityToDate { get; set; }
    public DateTime? ValidityFromDate { get; set; }
    public int? StuffId { get; set; }
    public int? CooperatorId { get; set; }
    public int? OrderId { get; set; }
    public int? EmployeeId { get; set; }

    public GetPriceAnnunciationInput(PagingInput pagingInput, PriceAnnunciationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
