using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.ProductionRequest
{
  public class GetProductionRequestInput : SearchInput<ProductionRequestSortType>
  {
    public GetProductionRequestInput(PagingInput pagingInput, ProductionRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public string Code { get; set; }
    public int? Version { get; set; }
    public int? StuffId { get; set; }
    public int? OrderItemId { get; set; }
    public System.DateTime? Deadline { get; set; }
    public ProductionRequestStatus? Status { get; set; }
    public ProductionRequestStatus[] Statuses { get; set; }
  }

}
