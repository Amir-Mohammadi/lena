using lena.Domains.Enums;
using lena.Models.ProjectManagement.ProjectERPDocument;
using lena.Models.ProjectManagement.ProjectERPLabelLog;
using lena.Models.ProjectManagement.ProjectERPResponsibleEmployee;
using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERP
{
  public class AddProjectERPInput
  {
    public string Title { get; set; }
    public string Code { get; set; }
    public short Version { get; set; }
    public ProjectERPPriority Priority { get; set; }
    public bool IsActive { get; set; }
    public short Progress { get; set; }
    public string Description { get; set; }
    public short ProjectERPCategoryId { get; set; }
    public Nullable<int> StuffId { get; set; }
    public Nullable<int> CustomerId { get; set; }
    public Nullable<short> ProjectERPPhaseId { get; set; }
    public Nullable<short> ProjectERPTypeId { get; set; }
    public Nullable<DateTime> EstimateStartDateTime { get; set; } // تاریخ تخمینی شروع پروژه
    public Nullable<DateTime> RealStartDateTime { get; set; } //تاریخ شروع واقعی

    public AddProjectERPResponsibleEmployeeInput[] AddProjectERPResponsibleEmployeeInputs { get; set; }
    public AddProjectERPLabelLogInput[] AddProjectERPLabelLogInputs { get; set; }
    public AddProjectERPDocumentInput[] AddProjectERPDocumentInputs { get; set; }


  }
}
