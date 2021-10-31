using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Supplies.StuffBasePriceCustoms;
using lena.Models.Supplies.StuffBasePriceTransport;
using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffBasePrice
{
  public class GetStuffBasePricesInput : SearchInput<StuffBasePriceSortType>
  {
    public GetStuffBasePricesInput(PagingInput pagingInput, StuffBasePriceSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? StuffId { get; set; }
    public StuffPriceStatus? Status { get; set; }
    public StuffPriceStatus[] Statuses { get; set; }
    public int? CurrencyId { get; set; }
    public int? UserId { get; set; }
    public AddStuffBasePriceCustomsInput Customs { get; set; }
    public AddStuffBasePriceTransportInput Transport { get; set; }
    public StuffPriceStatus[] NotHasStatuses { get; set; }
    public string Description { get; set; }
  }
}