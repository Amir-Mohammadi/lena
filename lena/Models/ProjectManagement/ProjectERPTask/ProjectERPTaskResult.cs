using lena.Domains.Enums;
using lena.Models.ProjectManagement.ProjectERPTaskDependency;
using lena.Models.ProjectManagement.ProjectERPTaskDocument;
using lena.Models.ProjectManagement.ProjectERPTaskLabelLog;
using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPTask
{
  public class ProjectERPTaskResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Output { get; set; }
    public ProjectERPTaskStatus Status { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? DueDateTime { get; set; }
    public int EstimateTime { get; set; }
    public int? DurationMinute { get; set; }
    public int? RemainingMinute => EstimateTime - DurationMinute;
    public int? ProgressPercentage { get; set; }
    public ProjectERPTaskPriority Priority { get; set; }
    public int ProjectERPId { get; set; }
    public int? AssigneeEmployeeId { get; set; }
    public string AssigneeEmployeeFullName { get; set; }
    public short ProjectERPTaskCategoryId { get; set; }
    public string ProjectERPTaskCategoryName { get; set; }
    public int? ParentTaskId { get; set; } // برای پیاده سازی ساختار WBS
    public DateTime CreateDateTime { get; set; }
    public int CreatorUserId { get; set; }
    public string CreatorEmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }

    public int ProjectERPTaskDocumentCount { get; set; }


    #region ICollection
    public IQueryable<ProjectERPTaskDependencyResult> ProjectERPTaskDependencies { get; set; }
    public IQueryable<ProjectERPTaskLabelLogResult> ProjectERPTaskLabelLogs { get; set; }
    public IQueryable<ProjectERPTaskDocumentResult> ProjectERPTaskDocuments { get; set; }
    #endregion

  }
}