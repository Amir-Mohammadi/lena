using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class NotificationGroup : IEntity
  {
    protected internal NotificationGroup()
    {
      this.Notifications = new HashSet<Notification>();
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
  }
}
