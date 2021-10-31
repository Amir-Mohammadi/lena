using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ScrumEntity : IEntity
  {
    protected internal ScrumEntity()
    {
      this.ScrumEntityLogs = new HashSet<ScrumEntityLog>();
      this.ScrumEntityComments = new HashSet<ScrumEntityComment>();
      this.ScrumEntityDocuments = new HashSet<ScrumEntityDocument>();
      this.StuffRequests = new HashSet<StuffRequest>();
      this.Notifications = new HashSet<Notification>();
      this.NextScrumEntityDependencies = new HashSet<ScrumEntityDependency>();
      this.RequisiteScrumEntityDependencies = new HashSet<ScrumEntityDependency>();
      this.GeneralStuffRequests = new HashSet<GeneralStuffRequest>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public bool IsCommit { get; set; }
    public long EstimatedTime { get; set; }
    public short DepartmentId { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsArchive { get; set; }
    public Nullable<int> BaseEntityId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ScrumEntityLog> ScrumEntityLogs { get; set; }
    public virtual Department Department { get; set; }
    public virtual ICollection<ScrumEntityComment> ScrumEntityComments { get; set; }
    public virtual ICollection<ScrumEntityDocument> ScrumEntityDocuments { get; set; }
    public virtual BaseEntity BaseEntity { get; set; }
    public virtual ICollection<StuffRequest> StuffRequests { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
    public virtual ICollection<ScrumEntityDependency> RequisiteScrumEntityDependencies { get; set; }
    public virtual ICollection<ScrumEntityDependency> NextScrumEntityDependencies { get; set; }
    public virtual ICollection<GeneralStuffRequest> GeneralStuffRequests { get; set; }
  }
}