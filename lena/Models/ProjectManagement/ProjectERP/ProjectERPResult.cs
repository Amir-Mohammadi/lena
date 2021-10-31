using lena.Domains.Enums;
using lena.Models.ProjectManagement.ProjectERPDocument;
using lena.Models.ProjectManagement.ProjectERPEvent;
using lena.Models.ProjectManagement.ProjectERPLabelLog;
using lena.Models.ProjectManagement.ProjectERPResponsibleEmployee;
using lena.Models.ProjectManagement.ProjectERPTask;
using System;
using System.Collections.Generic;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERP
{
  public class ProjectERPResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public int Version { get; set; }
    public ProjectERPPriority Priority { get; set; }
    public ProjectERPStatus Status { get; set; }
    public bool IsActive { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int? CustomerId { get; set; }
    public string CustomerName { get; set; }
    public short Progress { get; set; }
    public Nullable<short> ProjectERPPhaseId { get; set; }
    public string ProjectERPPhaseName { get; set; }
    public int CreatorUserId { get; set; }
    public string CreatorEmployeeFullName { get; set; }
    public DateTime CreateDateTime { get; set; }
    public Nullable<short> ProjectERPTypeId { get; set; }
    public string ProjectERPTypeName { get; set; }
    public short ProjectERPCategoryId { get; set; }
    public string ProjectERPCategoryName { get; set; }
    public Nullable<DateTime> EstimateStartDateTime { get; set; } // تاریخ تخمینی شروع پروژه
    public Nullable<DateTime> RealStartDateTime { get; set; } //تاریخ شروع واقعی
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }


    public int AllProjectERPTaskCount { get; set; }
    public int AllDoneProjectERPTaskCount { get; set; }
    public int AllDoingProjectERPTaskCount { get; set; }

    //public IEnumerable<string> ProjectERPLabelNames { get; set; }
    public IEnumerable<string> ProjectERPResponsibleEmployeeFullNames { get; set; }


    public IQueryable<ProjectERPResponsibleEmployeeResult> ProjectERPResponsibleEmployees { get; set; }
    public IQueryable<ProjectERPLabelLogResult> ProjectERPLabelLogs { get; set; }
    public IQueryable<ProjectERPEventResult> ProjectERPEvents { get; set; }
    public IQueryable<ProjectERPDocumentResult> ProjectERPDocuments { get; set; }
    public IQueryable<ProjectERPTaskResult> ProjectERPTasks { get; set; }

  }
}