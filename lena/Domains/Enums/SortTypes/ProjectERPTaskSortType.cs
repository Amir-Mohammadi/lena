using lena.Domains.Enums;
namespace lena.Domains.Enums.SortTypes
{
  public enum ProjectERPTaskSortType
  {
    Id,
    Title,
    Description,
    Output,
    StartDateTime,
    DueDateTime,
    EstimateTime,
    DurationMinute,
    ProgressPercentage,
    Priority,
    ProjectERPId,
    AssigneeEmployeeId,
    ProjectERPTaskCategoryId,
    ParentTaskId, // برای پیاده سازی ساختار WBS
    CreateDateTime,
    CreatorUserId,
    CreatorEmployeeFullName
  }
}
