using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkPlanStep
{
  public class GetWorkPlanStepsInput : SearchInput<WorkPlanStepSortType>
  {
    public GetWorkPlanStepsInput(PagingInput pagingInput, WorkPlanStepSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int[] WorkPlanIds { get; set; }
    public int? WorkPlanId { get; set; }
    public int? ProductionStepId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public int? BillOfMaterialStuffId { get; set; }
  }
}
