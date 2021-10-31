using lena.Domains.Enums;
using lena.Models.ProjectManagement.ProjectERPEventDocument;
using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPEvent
{
  public class ProjectERPEventResult
  {
    public int Id { get; set; }
    public int ProjectERPId { get; set; }
    public string ProjectERPTitle { get; set; }
    public short ProjectERPEventCategoryId { get; set; }
    public string ProjectERPEventCategoryName { get; set; }
    public int RegisterUserId { get; set; } // کاربر ثبت کننده
    public string RegisterEmployeeFullName { get; set; }
    public DateTime RegisterDateTime { get; set; } // تاریخ ثبت
    public Nullable<int> AudienceEmployeeId { get; set; } // کاربر مخاطب
    public string AudienceEmployeeFullName { get; set; }
    public string Audience { get; set; } // مخاطب خارج از سازمان
    public bool Confidential { get; set; } // محرمانه
    public string NextAction { get; set; } // اقدام بعدی
    public Nullable<DateTime> NextActionDateTime { get; set; } // تاریخ اقدام بعدی
    public string Description { get; set; }
    public Nullable<DateTime> CRMDateTime { get; set; }
    public ProjectERPEventType Type { get; set; }
    public ProjectERPEventAnnouncementType? AnnouncementType { get; set; } // نحوه اعلام
    public string AnnouncementDescription { get; set; }
    public byte[] RowVersion { get; set; }

    public int ProjectERPEventDocumentQty { get; set; }
    public IQueryable<ProjectERPEventDocumentResult> ProjectERPEventDocuments { get; set; }
  }
}