using lena.Domains.Enums;
using lena.Models.ProjectManagement.ProjectERPDocument;
using lena.Models.ProjectManagement.ProjectERPResponsibleEmployee;
using lena.Models.ProjectManagement.ProjectERPTask;
using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERP
{
  public class EditProjectERPInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public short Version { get; set; }
    public ProjectERPPriority Priority { get; set; }
    public bool IsActive { get; set; }
    public int? StuffId { get; set; }
    public int? CustomerId { get; set; }
    public short Progress { get; set; }
    public string Description { get; set; }
    public short ProjectERPCategoryId { get; set; }
    public Nullable<short> ProjectERPPhaseId { get; set; }
    public Nullable<short> ProjectERPTypeId { get; set; }
    public Nullable<DateTime> EstimateStartDateTime { get; set; } // تاریخ تخمینی شروع پروژه
    public Nullable<DateTime> RealStartDateTime { get; set; } //تاریخ شروع واقعی
    public byte[] RowVersion { get; set; }


    #region ICollection
    public AddProjectERPDocumentInput[] AddProjectERPDocumentInputs { get; set; }
    public DeleteProjectERPDocumentInput[] DeleteProjectERPDocumentInputs { get; set; }


    public AddProjectERPResponsibleEmployeeInput[] AddProjectERPResponsibleEmployeeInputs { get; set; }
    public DeleteProjectERPResponsibleEmployeeInput[] DeleteProjectERPResponsibleEmployeeInputs { get; set; }

    public AddProjectERPTaskInput[] AddProjectERPTaskInputs { get; set; }
    public DeleteProjectERPTaskInput[] DeleteProjectERPTaskInputs { get; set; }

    #endregion
  }
}
