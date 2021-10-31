using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Notification : IEntity
  {
    protected internal Notification()
    {
    }
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsSeen { get; set; }
    public DateTime RequestDate { get; set; }
    public Nullable<DateTime> SeenDate { get; set; }
    public int UserId { get; set; }
    public Nullable<int> ScrumEntityId { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<int> NotificationGroupId { get; set; }
    public virtual User User { get; set; }
    public virtual ScrumEntity ScrumEntity { get; set; }
    public virtual NotificationGroup NotificationGroup { get; set; }
  }
}
