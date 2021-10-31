using lena.Domains.Enums;
using lena.Models.ProjectManagement.ProjectERPTaskDependency;
using lena.Models.ProjectManagement.ProjectERPTaskLabelLog;
using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPTask
{
  public class AddProjectERPTaskInput
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public string Output { get; set; }
    public ProjectERPTaskStatus? Status { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? DueDateTime { get; set; }
    public int EstimateTime { get; set; }
    public int? DurationMinute { get; set; }
    public int? ProgressPercentage { get; set; }
    public ProjectERPTaskPriority Priority { get; set; }
    public int ProjectERPId { get; set; }
    public int? AssigneeEmployeeId { get; set; }
    public short ProjectERPTaskCategoryId { get; set; }
    public int? ParentTaskId { get; set; } // برای پیاده سازی ساختار WBS

    public string[] FileKeies { get; set; }
    public AddProjectERPTaskLabelLogInput[] AddProjectERPTaskLabelLogInputs { get; set; }
    public AddProjectERPTaskDependencyInput[] AddProjectERPTaskDependencyInputs { get; set; }

  }
}
