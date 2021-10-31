using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPEvent : IEntity
  {
    protected internal ProjectERPEvent()
    {
      this.ProjectERPEventDocuments = new HashSet<ProjectERPEventDocument>();
    }
    public int Id { get; set; }
    public int ProjectERPId { get; set; }
    public short ProjectERPEventCategoryId { get; set; }
    public int RegisterUserId { get; set; } // کاربر ثبت کننده
    public DateTime RegisterDateTime { get; set; } // تاریخ ثبت
    public Nullable<int> AudienceEmployeeId { get; set; } // کاربر مخاطب
    public string Audience { get; set; } // مخاطب خارج از سازمان
    public bool Confidential { get; set; } // محرمانه
    public string NextAction { get; set; } // اقدام بعدی
    public Nullable<DateTime> NextActionDateTime { get; set; } // تاریخ اقدام بعدی
    public string Description { get; set; }
    public Nullable<DateTime> CRMDateTime { get; set; }
    public ProjectERPEventType Type { get; set; }
    //public short ProjectERPEventActionTypeId { get; set; }
    public Nullable<ProjectERPEventAnnouncementType> AnnouncementType { get; set; } // نحوه اعلام   
    public string AnnouncementDescription { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User RegisterUser { get; set; }
    public virtual Employee AudienceEmployee { get; set; }
    public virtual ProjectERP ProjectERP { get; set; }
    public virtual ProjectERPEventCategory ProjectERPEventCategory { get; set; }
    public virtual ICollection<ProjectERPEventDocument> ProjectERPEventDocuments { get; set; }
    //public virtual ProjectERPEventActionType ProjectERPEventActionType { get; set; } //  این شاید حذف بشه
  }
}
