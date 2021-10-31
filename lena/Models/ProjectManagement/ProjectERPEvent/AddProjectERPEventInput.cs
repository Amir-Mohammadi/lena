using lena.Domains.Enums;
using lena.Models.ProjectManagement.ProjectERPEventDocument;
using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPEvent
{
  public class AddProjectERPEventInput
  {
    public int ProjectERPId { get; set; }
    public short ProjectERPEventCategoryId { get; set; }
    public Nullable<int> AudienceEmployeeId { get; set; } // کاربر مخاطب
    public string FileKey { get; set; }
    public string Audience { get; set; } // مخاطب خارج از سازمان
    public bool Confidential { get; set; } // محرمانه
    public string NextAction { get; set; } // اقدام بعدی
    public Nullable<DateTime> NextActionDateTime { get; set; } // تاریخ اقدام بعدی
    public Nullable<DateTime> CRMDateTime { get; set; }
    public ProjectERPEventType Type { get; set; }
    public ProjectERPEventAnnouncementType[] AnnouncementTypes { get; set; } // نحوه اعلام
    public string AnnouncementDescription { get; set; }
    public string Description { get; set; }

    public AddProjectERPEventDocumentInput[] AddProjectERPEventDocumentInputs { get; set; }

  }
}
