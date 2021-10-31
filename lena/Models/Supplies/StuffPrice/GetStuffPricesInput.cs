using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffPrice
{
  public class GetStuffPricesInput : SearchInput<StuffPriceSortType>
  {
    public GetStuffPricesInput(PagingInput pagingInput, StuffPriceSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? Id { get; set; }
    public string Code { get; set; }
    public int? StuffId { get; set; }
    public StuffPriceStatus? Status { get; set; }
    public StuffPriceStatus[] Statuses { get; set; }
    public int? CurrencyId { get; set; }
    public int? UserId { get; set; }
    public StuffPriceStatus[] NotHasStatuses { get; set; }
    public bool? IsCurrent { get; set; }
    public System.DateTime? FromDateTime { get; set; }
    public System.DateTime? ToDateTime { get; set; }

    public StuffPriceType? PriceType { get; set; }
  }
}