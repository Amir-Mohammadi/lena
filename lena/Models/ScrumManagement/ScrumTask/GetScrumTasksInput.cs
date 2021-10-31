using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;

using lena.Domains.Enums;
namespace lena.Models.ScrumManagement.ScrumTask
{
  public class GetScrumTasksInput : SearchInput<ScrumTaskSortType>
  {
    public short? DepartmentId { get; set; }
    public int? ScrumTaskTypeId { get; set; }
    public int? UserId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public ScrumTaskStep? ScrumTaskStep { get; set; }
    public ScrumTaskStep[] ScrumTaskSteps { get; set; }

    public GetScrumTasksInput(PagingInput pagingInput, ScrumTaskSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
