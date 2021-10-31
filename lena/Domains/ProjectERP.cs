using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERP : IEntity
  {
    protected internal ProjectERP()
    {
      this.ProjectERPTasks = new HashSet<ProjectERPTask>();
      this.ProjectERPEvents = new HashSet<ProjectERPEvent>();
      this.ProjectERPDocuments = new HashSet<ProjectERPDocument>();
      this.ProjectERPLabelLogs = new HashSet<ProjectERPLabelLog>();
      this.ProjectERPResponsibleEmployees = new HashSet<ProjectERPResponsibleEmployee>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public int Version { get; set; }
    public ProjectERPPriority Priority { get; set; }
    public ProjectERPStatus Status { get; set; }
    public bool IsActive { get; set; }
    public int? StuffId { get; set; }
    public int? CustomerId { get; set; }
    public short Progress { get; set; }
    public string Description { get; set; }
    public int CreatorUserId { get; set; }
    public DateTime CreateDateTime { get; set; }
    public short ProjectERPCategoryId { get; set; }
    public short? ProjectERPPhaseId { get; set; }
    public short? ProjectERPTypeId { get; set; }
    public DateTime? EstimateStartDateTime { get; set; } // تاریخ تخمینی شروع پروژه
    public DateTime? RealStartDateTime { get; set; } //تاریخ شروع واقعی
    public byte[] RowVersion { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual User CreatorUser { get; set; }
    public virtual Cooperator Customer { get; set; }
    public virtual ProjectERPType ProjectERPType { get; set; }
    public virtual ProjectERPPhase ProjectERPPhase { get; set; }
    public virtual ProjectERPCategory ProjectERPCategory { get; set; }
    public virtual ICollection<ProjectERPTask> ProjectERPTasks { get; set; }
    public virtual ICollection<ProjectERPEvent> ProjectERPEvents { get; set; }
    public virtual ICollection<ProjectERPDocument> ProjectERPDocuments { get; set; }
    public virtual ICollection<ProjectERPLabelLog> ProjectERPLabelLogs { get; set; }
    public virtual ICollection<ProjectERPResponsibleEmployee> ProjectERPResponsibleEmployees { get; set; }
  }
}
