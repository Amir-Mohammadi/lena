using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPTask : IEntity
  {
    protected internal ProjectERPTask()
    {
      this.ProjectERPTaskLabelLogs = new HashSet<ProjectERPTaskLabelLog>();
      this.ProjectERPTaskDocuments = new HashSet<ProjectERPTaskDocument>();
      this.ProjectERPTaskDependencies = new HashSet<ProjectERPTaskDependency>();
      this.PredecessorProjectERPTaskDependency = new HashSet<ProjectERPTaskDependency>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Output { get; set; }
    public ProjectERPTaskStatus Status { get; set; }
    public DateTime? StartDateTime { get; set; } //زمان شروع 
    public DateTime? DueDateTime { get; set; } // مهلت زمان انجام
    public int EstimateTime { get; set; } // زمان تخمینی
    public int? DurationMinute { get; set; } // مدت زمان صرف شده
    public int? ProgressPercentage { get; set; }
    public ProjectERPTaskPriority Priority { get; set; }
    public int ProjectERPId { get; set; }
    public int? AssigneeEmployeeId { get; set; }
    public short ProjectERPTaskCategoryId { get; set; }
    public int? ParentTaskId { get; set; } // برای پیاده سازی ساختار WBS
    public DateTime CreateDateTime { get; set; }
    public int CreatorUserId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User CreatorUser { get; set; }
    public virtual Employee AssigneeEmployee { get; set; }
    public virtual ProjectERP ProjectERP { get; set; }
    public virtual ProjectERPTaskCategory ProjectERPTaskCategory { get; set; }
    public virtual ICollection<EmployeeWorkReport> EmployeeWorkReports { get; set; }
    public virtual ICollection<ProjectERPTaskLabelLog> ProjectERPTaskLabelLogs { get; set; }
    public virtual ICollection<ProjectERPTaskDocument> ProjectERPTaskDocuments { get; set; }
    public virtual ICollection<ProjectERPTaskDependency> ProjectERPTaskDependencies { get; set; }
    public virtual ICollection<ProjectERPTaskDependency> PredecessorProjectERPTaskDependency { get; set; }
  }
}
