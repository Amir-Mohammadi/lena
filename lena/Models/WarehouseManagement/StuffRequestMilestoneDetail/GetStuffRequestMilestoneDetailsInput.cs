using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestMilestoneDetail
{
  public class GetStuffRequestMilestoneDetailsInput : SearchInput<StuffRequestMilestoneDetailSortType>
  {
    public GetStuffRequestMilestoneDetailsInput(PagingInput pagingInput, StuffRequestMilestoneDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? StuffRequestMilestoneId { get; set; }
    public int? UserId { get; set; }
    public int? StuffId { get; set; }
    public DateTime? FromDueDate { get; set; }
    public DateTime? ToDueDate { get; set; }

    public StuffRequestMilestoneDetailStatus? Status { get; set; }
    public StuffRequestMilestoneDetailStatus[] Statuses { get; set; }
  }
}
