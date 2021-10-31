using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkPlan
{
  public class GetWorkPlansInput : SearchInput<WorkPlanSortType>
  {
    public GetWorkPlansInput(PagingInput pagingInput, WorkPlanSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? Id { get; set; }
    public int? BillOfMaterialStuffId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
  }
}
