using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.Machine
{
  public class GetMachinesInput : SearchInput<MachineSortType>
  {
    public int? MachineTypeId { get; set; }
    public int[] MachineTypeIds { get; set; }
    public int? CanUseInWorkStationId { get; set; }
    public int? CanUseInProductionPlanIn { get; set; }

    public GetMachinesInput(PagingInput pagingInput, MachineSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
