

using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;


using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestMilestone
{
  public class GetStuffRequestMilestonesInput : SearchInput<StuffRequestMilestoneSortType>
  {
    public GetStuffRequestMilestonesInput(PagingInput pagingInput, StuffRequestMilestoneSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? FromDueDate { get; set; }
    public DateTime? ToDueDate { get; set; }
    public int? UserId { get; set; }
    public int? Id { get; set; }
    public StuffRequestMilestoneStatus? Status { get; set; }
    public StuffRequestMilestoneStatus[] Statuses { get; set; }
  }
}
