using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.PreparingSending
{
  public class GetPreparingSendsInput : SearchInput<PreparingSendingSortType>
  {
    public GetPreparingSendsInput(PagingInput pagingInput, PreparingSendingSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? SendPermissionId { get; set; }
    public int? CustomerId { get; set; }
    public int? OrderItemId { get; set; }
    public int? StuffId { get; set; }
    public int? OrderItemBlockId { get; set; }
    public PreparingSendingStatus? Status { get; set; }
    public PreparingSendingStatus[] Statuses { get; set; }
  }
}
