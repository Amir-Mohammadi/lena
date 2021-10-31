using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.ProjectManagement.ProjectERPTaskDependency;
using lena.Models.ProjectManagement.ProjectERPTaskLabelLog;
using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPTask
{
  public class EditProjectERPTaskInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Output { get; set; }
    public ProjectERPTaskStatus? Status { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? DueDateTime { get; set; }

    public int EstimateTime { get; set; }
    public int? DurationMinute { get; set; }
    public int ProgressPercentage { get; set; }
    public ProjectERPTaskPriority Priority { get; set; }
    public int ProjectERPId { get; set; }
    public int? AssigneeEmployeeId { get; set; }
    public short ProjectERPTaskCategoryId { get; set; }
    public int? ParentTaskId { get; set; }

    public AddProjectERPTaskDocumentInput[] AddProjectERPTaskDocumentInputs { get; set; }
    public DeleteProjectERPTaskDocumentInput[] DeleteProjectERPTaskDocumentInputs { get; set; }
    public AddProjectERPTaskLabelLogInput[] AddProjectERPTaskLabelLogInputs { get; set; }
    public AddProjectERPTaskDependencyInput[] AddProjectERPTaskDependencyInputs { get; set; }
    public DeleteProjectERPTaskDependencyInput[] DeleteProjectERPTaskDependencyInputs { get; set; }


    public byte[] RowVersion { get; set; }
  }
}
